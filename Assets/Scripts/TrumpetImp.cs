using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class TrumpetImpFormation {
    public HashSet<TrumpetImp> imps;

    private SortedSet<int> idQueue;

    private Vector2 myCenter;

    private float timer = 0.0f;

    Vector2[] arrangement =
    {
        new Vector2(0, 0),
        new Vector2(-2.0f, 0),
        new Vector2(2.0f, 0),
        new Vector2(0, -2.0f),
        new Vector2(0, 2.0f),
    };

    EnemyAffiliation affiliation;

    public TrumpetImpFormation(EnemyAffiliation affiliation)
    {
        imps = new HashSet<TrumpetImp>();
        idQueue = new SortedSet<int>();

        for(int i = 0; i < arrangement.Length; ++i)
        {
            idQueue.Add(i);
        }

        this.affiliation = affiliation;
    }

    public void update(TrumpetImp source)
    {
        if (source.currentFormationIndex != 0) return;

        if(timer > 0)
        {
            timer -= Time.fixedDeltaTime;
            return;
        }

        timer = Random.Range(2.5f, 3.5f);
        myCenter = (Vector2)source.currentTargetObject.transform.position + new Vector2(Random.Range(-3, 3), Random.Range(-3, 3));
    }

    public void addImp(TrumpetImp imp)
    {
        if (imp.myFormation == this) return;

        // Do not erroneously try to add imps if not possible.
        // This is a silent error; change this to an exception for debugging, maybe.
        if (!hasSpots()) return;

        if(imp.myFormation != null)
        {
            imp.myFormation.removeImp(imp);
        }

        imps.Add(imp);

        imp.currentFormationIndex = idQueue.Min;
        idQueue.Remove(idQueue.Min);

        imp.myFormation = this;
    }

    private void reassignImpIds()
    {
        foreach (TrumpetImp imp in imps)
        {
            idQueue.Add(imp.currentFormationIndex);
        }

        foreach (TrumpetImp imp in imps)
        {
            imp.currentFormationIndex = idQueue.Min;
            idQueue.Remove(idQueue.Min);
        }
    }

    public void removeImp(TrumpetImp imp)
    {
        imps.Remove(imp);
        if(imp.currentFormationIndex != -1)
            idQueue.Add(imp.currentFormationIndex);
        imp.currentFormationIndex = -1;

        // Reassign IDs to be sure that some imp has id 0, and that they are in order
        reassignImpIds();
    }


    public Vector2 getFormationPosition(int index)
    {
        if(index < 0)
        {
            return myCenter;
        }
        return myCenter + arrangement[index];
    }

    public int numSpotsRemaining()
    {
        return arrangement.Length - imps.Count;
    }

    public bool hasSpots()
    {
        return numSpotsRemaining() > 0;
    }

    public bool mayAdd(TrumpetImp imp)
    {
        if (!hasSpots()) return false;

        if (imp.affiliation != affiliation) return false;

        return true;
    }

    public bool isSingular()
    {
        return imps.Count == 1;
    }

    //// One single global formation for now for basic testing purposes
    //static TrumpetImpFormation theFormation = null;
    //public static TrumpetImpFormation getFormation()
    //{
    //    if(theFormation == null)
    //    {
    //        theFormation = new TrumpetImpFormation();
    //    }
    //    return theFormation;
    //}
}*/


public class TrumpetImp : BaseEnemy
{
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

        while(rows > 0)
        {
            if (desired > remaining) desired = remaining;

            float posX = (-0.5f * width);
            float deltaX = 0.0f;
            if(desired > 1)
            {
                deltaX = (width / (desired - 1));
            }
            else
            {
                posX = 0.0f;
            }

            for(int x = 0; x < desired; ++x)
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
            else if(health.gameObject.GetComponent<Player>() != null && affiliation == EnemyAffiliation.AgainstPlayer)
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
        if(targetVelocity.magnitude > maxNeededVelocity)
        {
            targetVelocity = targetVelocity.normalized * maxNeededVelocity;
        }

        float decelThreshold = rigidbody.velocity.sqrMagnitude / maxAcceleration;
        if (difference.magnitude < decelThreshold)
        {
            Vector2 accel = -rigidbody.velocity.normalized * maxAcceleration;
            accel *= Time.fixedDeltaTime;

            float maxFrameAccel = rigidbody.velocity.magnitude;
            if(accel.magnitude > maxFrameAccel)
            {
                accel = accel.normalized * maxFrameAccel;
            }

            rigidbody.AddForce(accel, ForceMode2D.Impulse);
            if(rigidbody.velocity.sqrMagnitude < 0.001 * 0.001)
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
            head = head.prevImp;

            if(cycleDetect++ > impsInFormation)
            {
                //Debug.Log("Find formation head cycle. SUs!!!");
                break;
            }
        }

        return head;
    }

    private TrumpetImp findFormationTail()
    {
        TrumpetImp tail = this;
        int cycleDetect = 0;
        while (tail.nextImp != null)
        {
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
        int cycleDetect = 0;

        // Must update the head before the formation may be correctly generated.
        head.impsInFormation = newSize;
        head.generateFormation();

        while (head != null)
        {
            head.impsInFormation = newSize;
            head = head.nextImp;

            if(cycleDetect++ > newSize)
            {
                //Debug.Log("Update formation size: Cycle. Sus!!!");
                break;
            }
        }
    }

    // Removes this imp from its current formation.
    private void leaveFormation()
    {
        TrumpetImp head = findFormationHead();

        updateFormationSize(head, impsInFormation - 1);

        // Update linked list.
        if(prevImp != null)
        {
            prevImp.nextImp = nextImp;
        }
        if(nextImp != null)
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

       // Debug.Log("formation join: " + this + " and " + someTargetImp);
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
        foreach(GameObject imp in imps)
        {
            if (imp == gameObject) continue;

            float dist = ((Vector2)imp.transform.position - (Vector2)transform.position).magnitude;

            

            if (dist <= radius)
            {
                bool f = testImpFormation(imp);
                if (f)
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

        if(closest != null)
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
        Color result = Color.red;
        if (affiliation == EnemyAffiliation.WithPlayer) result = Color.green;
        //if (affiliation == EnemyAffiliation.Blue) result = Color.blue;
        spriteRenderer.color = result;
    }

    private void retargetWholeFormation()
    {
        movePatternIndex += 1;
        if(movePatternIndex >= movePattern.Length)
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
            current.targetLocation = formationCenter + arrangement[(index % arrangement.Length)];

            index += 1;
            current = current.nextImp;

            if (index >= impsInFormation)
            {
                //Debug.Log("There was a cycle. Sus!!!");
                break;
            }
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
        retargetWholeFormation();
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
                if (be.affiliation != affiliation)
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
        if(isThereAnyAttackTarget())
        {
            attack();
        }
    }
	public new void Die()
	{
		leaveFormation();
		Destroy(gameObject);
	}
    private void FixedUpdate()
    {
        // Cooldown is updated each frame to time when the attack is ready.
        if(attackCooldown > 0) attackCooldown -= Time.fixedDeltaTime;

        checkForNearbyFriends();

        computeTarget();
        doXYPhysics();

        // Only formation index 0 actually does any updating.
        if(prevImp == null)
        {
            updateTheWholeFormation();
        }
       // if(myFormation != null)
        //    myFormation.update(this);

        UpdateColor();

        // Simple attacking logic for now: just attack whenever our cooldown is less than zero.
        if(attackCooldown <= 0)
        {
            // Only have a chance of attacking each frame that we are able to.
            if (Random.value <= 0.1)
            {
                tryAttacking();
            }
        }
    }

    protected override void OnAffiliationChanged(EnemyAffiliation old, EnemyAffiliation newA)
    {
        if (old != newA)
        {
            leaveFormation();
        }
    }
}
