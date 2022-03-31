using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrumpetImpFormation {
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
}


public class TrumpetImp : BaseEnemy
{
    /* The Rigidbody enables us to use the built-in physics system for collisions. */
    private Rigidbody2D rigidbody;

    /* The enemy's top speed, at least under normal conditions. */
    public float maxVelocity = 10;

    /* How fast the enemy accelerates to the top speed. */
    public float maxAcceleration = 5;

    private Vector2 targetLocation;

    public GameObject currentTargetObject = null;

    public int currentFormationIndex = -1;

    public TrumpetImpFormation myFormation = null;

    private SpriteRenderer spriteRenderer;

    private float aoeRadius;

    private ParticleSystem attackParticleSystem;

    private float attackCooldown = 0.5f;

    public int attackDamage = 1;

    private Health health;

    private void Start()
    {
        // MUST CALL PARENT START!
        base.Start();

        rigidbody = GetComponent<Rigidbody2D>();

        currentTargetObject = GameObject.FindWithTag("Player");

        // Everyone starts with their own formation by default.
        TrumpetImpFormation newFormation = new TrumpetImpFormation(affiliation);
        newFormation.addImp(this);

        spriteRenderer = GetComponent<SpriteRenderer>();

        aoeRadius = transform.FindChild("AOERadius").localPosition.magnitude;
        attackParticleSystem = GetComponent<ParticleSystem>();

        health = GetComponent<Health>();
    }

    private void attack()
    {
        foreach (Health health in GameObject.FindObjectsOfType<Health>())
        {
        
            bool shouldAttack = false;

            // Compare the given Health's position to our position.
            float radius = (health.transform.position - transform.position).magnitude;

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

        if (!ti.myFormation.mayAdd(this)) return false;

        //if (!ti.myFormation.hasSpots()) return false;

        if (ti.myFormation.isSingular()) return true;

        int fuzz_factor = 0;
        // Add fuzz to potentially leave current formation on occassion.
        if (Random.value < 0.01) fuzz_factor += 1;
        if (Random.value < 0.01) fuzz_factor += 1;

        return ti.myFormation.numSpotsRemaining() < (myFormation.numSpotsRemaining() + fuzz_factor);
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

            float dist = (imp.transform.position - transform.position).magnitude;

            bool f = testImpFormation(imp);

            if (dist <= radius && f)
            {
                if(closest == null)
                {
                    closest = imp;
                    maxDist = dist;
                }
                else if(dist < maxDist)
                {
                    closest = imp;
                    maxDist = dist;
                }
            }

        }

        if(closest != null)
        {
            TrumpetImp imp = closest.GetComponent<TrumpetImp>();
            if(imp.myFormation != null)
            {
                // Change my formation to be that one.
                imp.myFormation.addImp(this);
                
                
            }
        }
    }

    private void computeTarget()
    {
        if (myFormation == null) return;
        targetLocation = myFormation.getFormationPosition(currentFormationIndex);
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

    private void FixedUpdate()
    {
        // Cooldown is updated each frame to time when the attack is ready.
        if(attackCooldown > 0) attackCooldown -= Time.fixedDeltaTime;

        checkForNearbyFriends();

        computeTarget();
        doXYPhysics();

        // Only formation index 0 actually does any updating.
        if(myFormation != null)
            myFormation.update(this);

        UpdateColor();

        // Simple attacking logic for now: just attack whenever our cooldown is less than zero.
        if(attackCooldown <= 0)
        {
            attack();
        }

        if(health.currentHP <= 0)
        {
            myFormation.removeImp(this);
            Destroy(gameObject);
        }
    }

    protected override void OnAffiliationChanged(EnemyAffiliation old, EnemyAffiliation newA)
    {
        if (old != newA)
        {
            myFormation.removeImp(this);

            TrumpetImpFormation newFormation = new TrumpetImpFormation(newA);
            newFormation.addImp(this);
        }
    }
}
