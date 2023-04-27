using LibGameAI.FSMs;
using System;
using UnityEngine;

class FishAiBehaviour : MonoBehaviour
{
    [SerializeField] private float normalSpeed, fastSpeed, lungeSpeed;
    [SerializeField] private float normalAcceleration, lungeAcceleration, fastAcceleration;
    private float usingSpeed = 0, usingAcceleration = 0;

    private Vector3 target;
    private Fish fishInfo;

    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float tankHeight, tankLenght, tankWidht;

    private StateMachine fsm;
    [SerializeField] private float detectionRadius = 2f;

    private void Awake()
    {
        fishInfo = GetComponent<Fish>();
    }
    private void Start()
    {
        State chaseState = new State("Chase",
            () => { },
            Chase, // set the Chase method as the state's action
            () => { /* actions to perform when exiting the chase state */ });

        State idleState = new State("Idle",
            () => { },
            Idle, // set the Idle method as the state's action
            () => { /* actions to perform when exiting the idle state */ });


        State fleeState = new State("Flee",
           () => { /* actions to perform when entering the flee state */ },
           Flee, // set the flee method as the state's action
           () => { /* actions to perform when exiting the flee state */ });



        Transition transitionToChase = new Transition(
            () =>
            {
                if (fishInfo.FishType != FishType.small)
                {
                    // Get vector to closest target
                    Vector3 toClosest = GetClosestFish(false) - transform.position;

                    if (target != toClosest && fishInfo.Energy < 75)
                    {
                        target = toClosest;
                        return true;
                    }
                }

                if (fishInfo.FishType != FishType.big)
                {
                    // Get vector to closest target
                    Vector3 toClosest = GetClosestAlgae() - transform.position;

                    if (target != toClosest && fishInfo.Energy < 75)
                    {
                        target = toClosest;
                        return true;
                    }
                }

                return false;
            },
            () =>
            {
                RotateToTarget();
                MoveForward();
            },
            chaseState);

        Transition transitionToFlee = new Transition(
           () =>
           {
               // Get vector from the closest enemy
               Vector3 toClosest = transform.position - GetClosestFish();

               if (target != toClosest)
               {
                   target = toClosest;
                   return true;
               }
               return false;
           },
           () =>
           {
               RotateToTarget();
               MoveForward();
           },
           fleeState);

        Transition transitionToIdle = new Transition(
            () =>
            {
                if (fishInfo.Energy >= 75)
                {
                    return true;
                }
                return false;
            },
            () => { MoveForward(); },
            idleState);

        fsm = new StateMachine(idleState);
        chaseState.AddTransition(transitionToIdle);
        chaseState.AddTransition(transitionToFlee);
        chaseState.AddTransition(transitionToChase);


        idleState.AddTransition(transitionToIdle);
        chaseState.AddTransition(transitionToFlee);
        chaseState.AddTransition(transitionToChase);



    }

    private void FixedUpdate()
    {
        fsm.Update();
        Animate();
    }


    private void Chase()
    {

        //change maxSpeed and acceleration to chase/flee value
        usingSpeed = fastSpeed;
        usingAcceleration = fastAcceleration;

    }


    private void Idle()
    {

        //change maxSpeed and acceleration to idle value
        usingSpeed = 0;
        usingAcceleration = 0;

    }


    private void Flee()
    {
        //change maxSpeed and acceleration to chase/flee value
        usingSpeed = fastSpeed;
        usingAcceleration = fastAcceleration;

    }


    private Vector3 GetClosestFish(bool enemy = true)
    {
        Rigidbody best = null;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (Collider col in hitColliders)
        {
            Rigidbody targ = col.GetComponent<Rigidbody>();

            if (targ != null)
            {
                if (targ.tag == "Fish")
                {


                    if (enemy)
                    {
                        if (targ.transform.localScale.x > transform.localScale.x) //if fish isnt null and is greater than me  and is a fish
                        {
                            if (best == null)
                            {
                                best = targ;
                            }
                            else if (Vector3.Distance(transform.position, targ.position) < Vector3.Distance(best.position, transform.position))
                            {
                                best = best.transform.localScale.x < targ.transform.localScale.x ? targ : best; //if the new is greater than the old, change
                            }
                        }
                    }

                    else if (targ.transform.localScale.x < transform.localScale.x) //if not enemy that i'm looking for, then is algae?
                    {
                        if (best == null)
                        {
                            best = targ;
                        }
                        else if (Vector3.Distance(transform.position, targ.position) < Vector3.Distance(best.position, transform.position))
                        {
                            best = best.transform.localScale.x < targ.transform.localScale.x ? targ : best; //if the new is greater than the old, change
                        }
                    }
                }
            }

        }

        if (best == null) return transform.position;
        return best.position;


    }

    private Vector3 GetClosestAlgae()
    {
        Collider best = null;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (Collider col in hitColliders)
        {
            if (col.tag == "SeaWeed")
            {

                if (best == null)
                {
                    best = col;
                }
                else if (Vector3.Distance(transform.position, col.transform.position) < Vector3.Distance(best.transform.position, transform.position))
                {
                    best = best.transform.localScale.x < col.transform.localScale.x ? col : best; //if the new is greater than the old, change
                }

            }

        }

        if (best == null) return transform.position;
        return best.transform.position;

    }

    private void MoveForward()
    {

        rb.AddForce(target.normalized * usingAcceleration);

        // Limit the maximum speed of the object
        if (rb.velocity.magnitude > usingSpeed)
        {
            rb.velocity = rb.velocity.normalized * usingSpeed;
        }


    }

    private void RotateToTarget()
    {
        transform.rotation =
            Quaternion.LookRotation(target - transform.position);
    }


    private void Animate()
    {
        if ((Vector3.Distance(target, transform.position) > 0.5f))
        {

            if (Vector3.Distance(target, transform.position) < 1)
            {
                anim.SetFloat("Speed", 1);
            }
            else
            {
                anim.SetFloat("Speed", Vector3.Distance(target, transform.position));
            }

        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

}
