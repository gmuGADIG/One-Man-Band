using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrumpetImp : BaseEnemy
{
    /// <summary>
    /// The radius within which the imp is allowed to retarget the player. If the imps don't retarget the player,
    /// they will simply idle in their current position.
    /// 
    /// A value of 20 seems to be reasonable, possibly.
    /// </summary>
    public float allowToTargetPlayerRadius = 20;

    // To keep track of formations: Each imp keeps track of the next imp in the formation, and the previous one.
    // The imp that has no previous imp is responsible for controlling the whole formation.
    private TrumpetImp nextImp = null;
    private TrumpetImp prevImp = null;

    int impsInFormation = 1;

    /* The Rigidbody enables us to use the built-in physics system for collisions. */
    private Rigidbody2D rigidbody;

    /* The enemy's top speed, at least under normal conditions. */
    public float maxVelocity = 10;

    /* How fast the enemy accelerates to the top speed. */
    public float maxAcceleration = 5;

    private Vector2 targetLocation;

    public GameObject currentTargetObject = null;

    public int currentFormationIndex = -1;

    // public TrumpetImpFormation myFormation = null;

    private SpriteRenderer spriteRenderer;

    private float aoeRadius;

    private ParticleSystem attackParticleSystem;

    private float attackCooldown = 0.5f;

    public int attackDamage = 1;

    private Health health;

    Animator animator;

    public AudioClip[] attackAudio;

    Vector2[] arrangement =
    {
        new Vector2(0, 0)
    };

    Vector2[] movePattern = {
        new Vector2(-3.0f,  3.0f),
        new Vector2(-2.0f,  3.0f),
        new Vector2(-1.0f,  3.0f),
        new Vector2( 0.0f,  3.0f),
        new Vector2( 1.0f,  3.0f),
        new Vector2( 2.0f,  3.0f),
        new Vector2( 3.0f,  3.0f),

        new Vector2( 3.0f,  2.0f),
        new Vector2( 2.0f,  2.0f),
        new Vector2( 2.0f,  1.0f),
        new Vector2( 1.0f,  1.0f),
        new Vector2( 1.0f,  0.0f),
        new Vector2( 0.0f,  0.0f),
        new Vector2(-1.0f,  0.0f),
        new Vector2(-1.0f, -1.0f),
        new Vector2(-2.0f, -1.0f),
        new Vector2(-2.0f, -2.0f),
        new Vector2(-3.0f, -2.0f),

        new Vector2(-3.0f, -3.0f),
        new Vector2(-2.0f, -3.0f),
        new Vector2(-1.0f, -3.0f),
        new Vector2( 0.0f, -3.0f),
        new Vector2( 1.0f, -3.0f),
        new Vector2( 2.0f, -3.0f),
        new Vector2( 3.0f, -3.0f),
    };

    float movePatternSize = 0.6f;

    private void generateFormation()
    {
        if (impsInFormation < 1) return;

        // Debug.Log(" --- Generate Arrangement: " + impsInFormation + " --- ");

        int rows = Mathf.CeilToInt(Mathf.Sqrt(impsInFormation));
        int remaining = impsInFormation;

        int desired = Mathf.CeilToInt((float)remaining / (float)rows);

        arrangement = new Vector2[impsInFormation];

        float width = rows;

        float posY = ((width - 1.0f) / 2.0f);

        int nextWrite = 0;

        while (rows > 0)
        {
            if (desired > remaining) desired = remaining;

            float posX = (-0.5f * width);
            float deltaX = 0.0f;
            if (desired > 1)
            {
                deltaX = (width / (desired - 1));
            }
            else
            {
                posX = 0.0f;
            }

            for (int x = 0; x < desired; ++x)
            {
                arrangement[nextWrite++] = new Vector2(posX, posY) * 2.0f;
                //Debug.Log("Added pos: " + arrangement[nextWrite - 1]);
                posX += deltaX;
            }

            remaining -= desired;
            rows -= 1;

            posY *= -1.0f;

            if (desired > remaining) desired = remaining;

            posX = (-0.5f * width);
            deltaX = 0.0f;
            if (desired > 1)
            {
                deltaX = (width / (desired - 1));
            }
            else
            {
                posX = 0.0f;
            }

            for (int x = 0; x < desired; ++x)
            {
                arrangement[nextWrite++] = new Vector2(posX, posY) * 2.0f;
                //Debug.Log("Added pos: " + arrangement[nextWrite - 1]);
                posX += deltaX;
            }

            remaining -= desired;
            rows -= 1;

            if (rows > 0)
            {
                desired = Mathf.CeilToInt((float)remaining / (float)rows);
            }

            posY *= -1.0f;
            posY -= 1.0f;
        }
    }
    private void Start()
    {
        // MUST CALL PARENT START!
        base.Start();

        rigidbody = GetComponent<Rigidbody2D>();

        currentTargetObject = GameObject.FindWithTag("Player");

        spriteRenderer = GetComponent<SpriteRenderer>();

        aoeRadius = transform.Find("AOERadius").localPosition.magnitude;
        attackParticleSystem = GetComponent<ParticleSystem>();

        animator = GetComponent<Animator>();

        health = GetComponent<Health>();
    }

    private void attack()
    {
        foreach (Health health in GameObject.FindObjectsOfType<Health>())
        {

            bool shouldAttack = false;

            // Compare the given Health's position to our position.
            // IMPORTANT: These must be cast to Vector2 so taht the Z component does not influence the magnitude.
            float radius = ((Vector2)health.transform.position - (Vector2)transform.position).magnitude;

            // Never attack anything outside the radius. Early exit, so to speak.
            if (radius > aoeRadius) continue;

            BaseEnemy be = health.gameObject.GetComponent<BaseEnemy>();
            if (be != null)
            {
                if (be.affiliation != affiliation)
                {
                    shouldAttack = true;
                }
            }
            else if (health.gameObject.GetComponent<Player>() != null && affiliation == EnemyAffiliation.AgainstPlayer)
            {
                shouldAttack = true;
            }


            if (shouldAttack)
            {
                // Damage any relevant enemies.
                health.Damage(attackDamage);
            }
        }

        attackParticleSystem.Play();
        attackCooldown = 1.0f;
        animator.SetTrigger("attack");
    }

    /// <summary>
    /// Performs the primary physical movement of the player--in particular, accelerating in the
    /// X and Y directions (but does not handle the jumping and its Z component).
    /// </summary>
    private void doXYPhysics()
    {
        Vector2 difference = (targetLocation - rigidbody.position);
        Vector2 targetVelocity = difference.normalized * maxVelocity;
        float maxNeededVelocity = (difference.magnitude / Time.fixedDeltaTime);
        if (targetVelocity.magnitude > maxNeededVelocity)
        {
            targetVelocity = targetVelocity.normalized * maxNeededVelocity;
        }

        float decelThreshold = rigidbody.velocity.sqrMagnitude / maxAcceleration;
        if (difference.magnitude < decelThreshold)
        {
            Vector2 accel = -rigidbody.velocity.normalized * maxAcceleration;
            accel *= Time.fixedDeltaTime;

            float maxFrameAccel = rigidbody.velocity.magnitude;
            if (accel.magnitude > maxFrameAccel)
            {
                accel = accel.normalized * maxFrameAccel;
            }

            rigidbody.AddForce(accel, ForceMode2D.Impulse);
            if (rigidbody.velocity.sqrMagnitude < 0.001 * 0.001)
            {
                rigidbody.velocity = new Vector2(0, 0);
            }
        }
        else
        {
            // The maximum acceleration we can perform this frame is in totalAccel, as it represents
            // an acceleration to the exact target velocity.
            Vector2 totalAccel = (targetVelocity - rigidbody.velocity);

            // Compute the actual acceleration, and multiply by fixedDeltaTime. This enables clamping
            // the delta-multiplied acceleration if it exceeds the maximum.
            Vector2 accel = (totalAccel.normalized * maxAcceleration) * Time.fixedDeltaTime;

            if (accel.magnitude > totalAccel.magnitude)
            {
                // If the accel magnitude is too large, then we only want to accelerate up to
                // totalAccel length. So, we just copy the value over.
                accel = totalAccel;
            }

            // This is essentially equivalent to velocity += accel (which is, remember, already multiplied by fixedDeltaTime)
            // So it is integrating to find velocity, but doing it with a rigidbody enables collisions.
            rigidbody.AddForce(accel, ForceMode2D.Impulse);
        }
    }



    bool testImpFormation(GameObject imp)
    {
        TrumpetImp ti = imp.GetComponent<TrumpetImp>();
        if (ti == null) return false;

        if (ti == this) return false;

        if (ti.affiliation != affiliation) return false;

        // TODO: More specific max formation size.
        if (ti.impsInFormation >= 20) return false;

        TrumpetImp myHead = findFormationHead();
        TrumpetImp otherHead = ti.findFormationHead();
        if (myHead == otherHead) return false;

        // We always join if we are alone.
        if (impsInFormation == 1) return true;
        //if (ti.myFormation.isSingular()) return true;

        int fuzz_factor = 0;
        // Add fuzz to potentially leave current formation on occassion.
        if (Random.value < 0.01) fuzz_factor += 1;
        if (Random.value < 0.01) fuzz_factor += 1;

        return impsInFormation < (ti.impsInFormation + fuzz_factor);
    }

    // FInds the head of this formation, for updating the imp count.
    private TrumpetImp findFormationHead()
    {
        TrumpetImp head = this;
        int cycleDetect = 0;
        while (head.prevImp != null)
        {
            if (cycleDetect >= impsInFormation)
            {
                //Debug.Log("Cycle in Find Head: " + cycleDetect + " vs " + impsInFormation);
                break;
            }

            head = head.prevImp;

            cycleDetect++;
        }

        return head;
    }

    private TrumpetImp findFormationTail()
    {
        TrumpetImp tail = this;
		TrumpetImp head = this.prevImp;
		int cycleDetect = 0;
        while (tail.nextImp != null)
        {
			if(tail.nextImp == prevImp)
			{
				break;
			}
            tail = tail.nextImp;

            if (cycleDetect++ > impsInFormation)
            {
                //Debug.Log("Find formation tail cycle. SUs!!!");
                break;
            }
        }

        return tail;
    }

    // Updates the formation size for a head imp.
    private void updateFormationSize(TrumpetImp head, int newSize)
    {
        //Debug.Log("Updating formation @ " + head + " to size " + newSize);

        int cycleDetect = 0;

        // In e.g. leave formation, we will be updating more nodes than will
        // actually exist.
        // This is because we update the size before relinking the list.
        int nodesToUpdate = head.impsInFormation;
		TrumpetImp tail = head.prevImp;
        // Must update the head before the formation may be correctly generated.
        head.impsInFormation = newSize;
        head.generateFormation();

        while (head != null)
        {
			if (head.nextImp = tail) { break; }
            if (cycleDetect >= nodesToUpdate)
            {
                //Debug.Log("Cycle in Update Size: " + cycleDetect + " vs " + newSize);
                break;
            }

            head.impsInFormation = newSize;
            head = head.nextImp;

            cycleDetect++;
        }
    }

    // Removes this imp from its current formation.
    private void leaveFormation()
    {
        TrumpetImp head = findFormationHead();

        updateFormationSize(head, impsInFormation - 1);

        // Update linked list.
        if (prevImp != null)
        {
            prevImp.nextImp = nextImp;
        }
        if (nextImp != null)
        {
            nextImp.prevImp = prevImp;
        }

        // Join a new empty formation.
        prevImp = null;
        nextImp = null;
        impsInFormation = 1;

        generateFormation();

        // Empty formation has short retarget timer
        formationRetargetTimer = 0.1f;
    }

    private void joinFormation(TrumpetImp someTargetImp)
    {
        if (someTargetImp == this) return;

        TrumpetImp myHead = findFormationHead();
        TrumpetImp otherHead = someTargetImp.findFormationHead();

        if (myHead == otherHead) return;

        // Cannot be in a formation when joining a new one.
        leaveFormation();

        //Debug.Log("formation join: " + this + " and " + someTargetImp);
        //Debug.Log("    note: prevous heads: " + myHead + " vs " + otherHead);

        // Insert after someTargetImp.
        nextImp = someTargetImp.nextImp;
        prevImp = someTargetImp;

        someTargetImp.nextImp = this;

        if (nextImp != null)
        {
            nextImp.prevImp = this;
        }

        //Debug.Log("Sanity check: (prev.next == this): " + (prevImp.nextImp == this));
        //if(nextImp != null)
        //     Debug.Log("Sanity check: (next.prev == this): " + (nextImp.prevImp == this));

        // Need to update formation size.
        updateFormationSize(otherHead, someTargetImp.impsInFormation + 1);

        //Debug.Log("Joined formation with size: " + impsInFormation);
        //Debug.Log("My new head: " + findFormationHead());
    }

    private void checkForNearbyFriends()
    {
        GameObject[] imps = GameObject.FindGameObjectsWithTag("TrumpetImp");

        float radius = 2.0f;

        GameObject closest = null;
        float maxDist = -1.0f;


        // TODO: Only do this once per frame, for all imps.
        // That will still be O(n^2) but it will be much better.
        // Even better option: Use colliders or something..?
        foreach (GameObject imp in imps)
        {
            if (imp == gameObject) continue;

            float dist = ((Vector2)imp.transform.position - (Vector2)transform.position).magnitude;



            if (dist <= radius)
            {
                bool f = testImpFormation(imp);
                if (f && imp.GetComponent<TrumpetImp>().affiliation == affiliation)
                {
                    if (closest == null)
                    {
                        closest = imp;
                        maxDist = dist;
                    }
                    else if (dist < maxDist)
                    {
                        closest = imp;
                        maxDist = dist;
                    }
                }
            }

        }

        if (closest != null)
        {
            // Change my formation to be that one.
            joinFormation(closest.GetComponent<TrumpetImp>());
            //TrumpetImp imp = closest.GetComponent<TrumpetImp>();
            //if(imp.myFormation != null)
            //{

            //   imp.myFormation.addImp(this);


            //}
        }
    }

    private void computeTarget()
    {
        //if (myFormation == null) return;
        //targetLocation = myFormation.getFormationPosition(currentFormationIndex);
        //if(currentTargetObject != null)
        //{
        //targetLocation = (Vector2)currentTargetObject.transform.position;
        //}
    }

    private void UpdateColor()
    {
        // Color result = Color.red;
        //  if (affiliation == EnemyAffiliation.WithPlayer) result = Color.green;
        //if (affiliation == EnemyAffiliation.Blue) result = Color.blue;
        spriteRenderer.color = Color.white;
    }

    private void retargetWholeFormation()
    {

        movePatternIndex += 1;
        if (movePatternIndex >= movePattern.Length)
        {
            movePatternIndex = 0;
            movePatternCenter = (Vector2)currentTargetObject.transform.position;

            int offsetX = Mathf.RoundToInt(Random.RandomRange(-5.3f, 5.3f));
            int offsetY = Mathf.RoundToInt(Random.RandomRange(-5.3f, 5.3f));

            // Random offsets for marching.
            movePatternCenter.x += offsetX * movePatternSize;
            movePatternCenter.y += offsetY * movePatternSize;

            formationCenter = movePatternCenter + movePatternSize * movePattern[movePatternIndex];

            float dist = ((Vector2)transform.position - formationCenter).magnitude;
            formationRetargetTimer = (dist / maxVelocity) + 0.1f;
        }
        else
        {
            formationCenter = movePatternCenter + movePatternSize * movePattern[movePatternIndex];

            float dist = ((Vector2)transform.position - formationCenter).magnitude;
            formationRetargetTimer = (dist / maxVelocity) + 0.1f;
        }

    }

    private void updateFormationTargetsPerFrame()
    {
        TrumpetImp current = this;

        int index = 0;

        while (current != null)
        {
            if (index >= impsInFormation)
            {
                //Debug.Log("Imp Debug: " + index + " vs " + impsInFormation);
                break;
            }

            current.targetLocation = formationCenter + arrangement[(index % arrangement.Length)];

            index += 1;
            current = current.nextImp;


        }
    }

    float formationRetargetTimer = -1.0f;
    Vector2 formationCenter = Vector2.zero;

    Vector2 movePatternCenter = Vector2.zero;
    int movePatternIndex = 100000;

    private void updateTheWholeFormation()
    {
        updateFormationTargetsPerFrame();
        //currentTargetObject.transform.position + new Vector2(Random.Range(-3, 3), Random.Range(-3, 3));
        if (formationRetargetTimer > 0)
        {
            formationRetargetTimer -= Time.fixedDeltaTime;
            return;
        }

        //formationRetargetTimer = Random.Range(2.5f, 3.5f);

        Vector2 tar = (Vector2)currentTargetObject.transform.position;
        Vector2 me = (Vector2)transform.position;

        // Debug.Log("Current dist: " + Vector2.Distance(tar, me) + " vs " + allowToTargetPlayerRadius);

        if (Vector2.Distance(tar, me) <= allowToTargetPlayerRadius)
        {
            // Debug.Log("Retarget! Lmao...");

            retargetWholeFormation();
        }
    }

    private bool isThereAnyAttackTarget()
    {
        Health[] healths = GameObject.FindObjectsOfType<Health>();
        //Debug.Log("Found " + healths.Length + " healths");
        foreach (Health possibleEnemyHealth in healths)
        {

            bool shouldAttack = false;

            // Compare the given Health's position to our position.
            float radius = ((Vector2)possibleEnemyHealth.transform.position - (Vector2)transform.position).magnitude;

            // We are checking if there's anything in the radius.
            if (radius > aoeRadius) continue;



            BaseEnemy be = possibleEnemyHealth.GetComponent<BaseEnemy>();
            if (be != null)
            {
                if ((int)be.affiliation == ((int)affiliation + 1 % 3))
                {
                    shouldAttack = true;
                }
            }
            else if (possibleEnemyHealth.GetComponent<Player>() != null && affiliation == EnemyAffiliation.AgainstPlayer)
            {
                shouldAttack = true;
            }

            if (shouldAttack)
            {
                return true;
            }
        }

        return false;
    }

    private void tryAttacking()
    {
        if (isThereAnyAttackTarget())
        {
            attack();

            this.gameObject.GetComponent<AudioSource>().PlayOneShot(attackAudio[Random.Range(0, attackAudio.Length)]);
        }
    }
    private void OnDestroy()
    {
        /* MUST leave the formation when the gameobject is destroyed! */
        leaveFormation();
    }

    private void FixedUpdate()
    {
		if(Vector2.Distance(transform.position, Target.transform.position) > allowToTargetPlayerRadius)
		{
			return;
		}
        // Cooldown is updated each frame to time when the attack is ready.
        if (attackCooldown > 0) attackCooldown -= Time.fixedDeltaTime;

        checkForNearbyFriends();

        computeTarget();
        doXYPhysics();

        // Only formation index 0 actually does any updating.
        if (prevImp == null)
        {
            updateTheWholeFormation();
        }
        // if(myFormation != null)
        //    myFormation.update(this);

        UpdateColor();

        // Simple attacking logic for now: just attack whenever our cooldown is less than zero.
        if (attackCooldown <= 0)
        {
            // Only have a chance of attacking each frame that we are able to.
            if (Random.value <= 0.1)
            {
                tryAttacking();
            }
        }

        animator.SetFloat("velX", rigidbody.velocity.x);
        animator.SetFloat("velY", rigidbody.velocity.y);
        animator.SetBool("Vert", Mathf.Abs(rigidbody.velocity.y) > Mathf.Abs(rigidbody.velocity.x));
    }

    protected override void OnAffiliationChanged(EnemyAffiliation old, EnemyAffiliation newA)
    {
        if (old != newA)
        {
            leaveFormation();
        }
    }
}
