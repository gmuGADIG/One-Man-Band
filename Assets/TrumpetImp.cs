using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrumpetImpFormation {
    public HashSet<TrumpetImp> imps;

    private Vector2 myCenter;

    private float timer = 0.0f;

    public TrumpetImpFormation()
    {
        imps = new HashSet<TrumpetImp>();
    }

    public void update(TrumpetImp source)
    {
        if (source.currentFormationIndex != 0) return;

        if(timer > 0)
        {
            timer -= Time.fixedDeltaTime;
            return;
        }

        timer = 3.0f;
        myCenter = (Vector2)source.currentTargetObject.transform.position;
    }

    public void addImp(TrumpetImp imp)
    {
        if (imp.myFormation == this) return;

        if(imp.myFormation != null)
        {
            imp.myFormation.removeImp(imp);
        }

        imps.Add(imp);
        imp.currentFormationIndex = imps.Count - 1;
        imp.myFormation = this;
    }

    public void removeImp(TrumpetImp imp)
    {
        imps.Remove(imp);
    }

    Vector2[] arrangement =
    {
        new Vector2(0, 0),
        new Vector2(-2.0f, 0),
        new Vector2(2.0f, 0),
        new Vector2(0, -2.0f),
        new Vector2(0, 2.0f),
    };

    public Vector2 getFormationPosition(int index)
    {
        return myCenter + arrangement[index];
    }

    public bool hasSpots()
    {
        return imps.Count <= 4;
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


public class TrumpetImp : MonoBehaviour
{
    /* The Rigidbody enables us to use the built-in physics system for collisions. */
    private Rigidbody2D rigidbody;

    /* The enemy's top speed, at least under normal conditions. */
    public float maxVelocity = 10;

    /* How fast the enemy accelerates to the top speed. */
    public float maxAcceleration = 5;

    private Vector2 targetLocation;

    public GameObject currentTargetObject = null;

    public int currentFormationIndex = 0;

    public TrumpetImpFormation myFormation = null;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        currentTargetObject = GameObject.FindWithTag("Player");

        // Everyone starts with their own formation by default.
        TrumpetImpFormation newFormation = new TrumpetImpFormation();
        newFormation.addImp(this);
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


    static bool isInNonfullFormation(GameObject imp)
    {
        TrumpetImp ti = imp.GetComponent<TrumpetImp>();
        if (ti == null) return false;

        return ti.myFormation.hasSpots();
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

            if(dist <= radius && isInNonfullFormation(imp))
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
                //print("JOINED A NEW FORMATION!!");
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

    private void FixedUpdate()
    {
        checkForNearbyFriends();

        computeTarget();
        doXYPhysics();

        // Only formation index 0 actually does any updating.
        if(myFormation != null)
            myFormation.update(this);
    }
}
