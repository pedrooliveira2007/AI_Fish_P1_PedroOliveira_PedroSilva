using LibGameAI.FSMs;
using System;
using UnityEngine;

class FishAiBehaviour : MonoBehaviour
{
    [SerializeField] private float normalSpeed, fastSpeed, lungeSpeed;
    [SerializeField] private float normalAcceleration, lungeAcceleration, fastAcceleration;
    private float usingSpeed = 0, usingAcceleration = 0;

    private Transform target;
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
        State pursueState = new State("Pursue",
            () => { Debug.Log("pur");/* actions to perform when entering the pursue state */},
            Pursue, // set the Chase method as the state's action
            () => { /* actions to perform when exiting the pursue state */ }
            );

        State idleState = new State("Idle",
            () => { Debug.Log("idle"); /* actions to perform when entering the idle state */ },
           Idle,
            () => { /* actions to perform when exiting the idle state */ }
            );


        State evadeState = new State("Evade",
           () => { Debug.Log("evade");/* actions to perform when entering the evade state */ },
           Evade, // set the evade method as the state's action
           () => { fishInfo.BeingChased(false); }
           );


        State consumeState = new State("Consume",
           () => { Debug.Log("consume"); /* actions to perform when entering the consume state */ },
           Consume, // set the consume method as the state's action
           () => {/* actions to perform when exiting the consume state */ }
           );


        State reproduceState = new State("Reproduce",
           () => { Debug.Log("repro");/* actions to perform when entering the consume state */ },
           Reproduce, // set the consume method as the state's action
           () => {/* actions to perform when exiting the consume state */ }
           );


        Transition transitionToPursue = new Transition(
            () => (fishInfo.Energy < 100 && !fishInfo.Chased),
            () => { },
            pursueState);

        Transition transitionToEvade = new Transition(
           () => fishInfo.Chased,
           () => { },
           evadeState);

        Transition transitionToIdle = new Transition(
            () => (fishInfo.Energy >= 75),
            () => { },
            idleState);

        Transition transitionToConsume = new Transition(
            () => (target != null && Vector3.Distance(transform.position, target.position) < 0.005f),
            () => { },
           consumeState);


        Transition transitionToReproduce = new Transition(
            () => (fishInfo.Energy >= 100 && !fishInfo.Chased),
            () => { },
            reproduceState);

        fsm = new StateMachine(idleState);
        pursueState.AddTransition(transitionToIdle);
        pursueState.AddTransition(transitionToEvade);
        pursueState.AddTransition(transitionToConsume);
        pursueState.AddTransition(transitionToReproduce);

        idleState.AddTransition(transitionToPursue);
        idleState.AddTransition(transitionToEvade);
        idleState.AddTransition(transitionToReproduce);

        consumeState.AddTransition(transitionToPursue);
        consumeState.AddTransition(transitionToEvade);
        consumeState.AddTransition(transitionToIdle);

        evadeState.AddTransition(transitionToPursue);
        evadeState.AddTransition(transitionToIdle);
    }

    private void Reproduce()
    {
        fishInfo.Reproduce();
    }

    private void FixedUpdate()
    {
        Action actions = fsm.Update();
        actions?.Invoke();
        if (target != null)
        {
            RotateToTarget();
        }
    }

    private void Update()
    {
        Animate();

    }

    private void Pursue()
    {

        if (fishInfo.FishType != FishType.big)
        {
            // Get vector to closest target
            Transform toClosest = GetClosestAlgae();

            if (target != toClosest && fishInfo.Energy < 75)
            {
                target = toClosest;
            }
        }

        if (fishInfo.FishType != FishType.small)
        {
            // Get vector to closest target
            Transform toClosest = GetClosestFish(false);

            if (target != toClosest)
            {
                target = toClosest;
            }
        }
        //change maxSpeed and acceleration to chase/flee value
        usingSpeed = fastSpeed;
        usingAcceleration = fastAcceleration;
        Debug.Log(target.position);
        MoveForward();
    }


    private void Idle()
    {

        //change maxSpeed and acceleration to idle value
        usingSpeed = 0;
        usingAcceleration = 0;
        MoveForward();
    }

    private void Consume()
    {

        fishInfo.Eat(target);

    }

    private void Evade()
    {
        //change maxSpeed and acceleration to chase/flee value
        usingSpeed = fastSpeed;
        usingAcceleration = fastAcceleration;

        MoveForward();
    }


    private Transform GetClosestFish(bool enemy = true)
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

        if (best == null) return transform;
        return best.transform;


    }

    private Transform GetClosestAlgae()
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

        if (best == null) return transform;
        return best.transform;

    }

    private void MoveForward()
    { 
        Vector3 move = Vector3.zero;
        if(target != null)
        {
            move = (target.position - transform.position) * usingSpeed * Time.fixedDeltaTime;
        }
       

        rb.velocity = new Vector3(move.x, move.y, move.z);
        // Limit the maximum speed of the object
        if (rb.velocity.magnitude > usingSpeed)
        {
            rb.velocity = rb.velocity.normalized * usingSpeed;
        }


    }

    private void RotateToTarget()
    {
        transform.rotation =
            Quaternion.LookRotation(target.position - transform.position);
    }


    private void Animate()
    {
        if (target == null)
        {
            anim.SetFloat("Speed", 1);
        }
        else if ((Vector3.Distance(target.position, transform.position) > 0.5f))
        {

            if (Vector3.Distance(target.position, transform.position) < 1)
            {
                anim.SetFloat("Speed", 1);
            }
            else
            {
                anim.SetFloat("Speed", Vector3.Distance(target.position, transform.position));
            }

        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

}
