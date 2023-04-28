using LibGameAI.FSMs;
using System;
using UnityEngine;

class FishAiBehaviour : MonoBehaviour
{
    [SerializeField] private float normalSpeed, fastSpeed;
    [SerializeField] private float normalAcceleration, fastAcceleration;
    private float usingSpeed = 0;

    private Transform target;
    private Fish fishInfo;
    private bool away = false;
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
           () => { Consume(); /* actions to perform when entering the consume state */ },
           Consume, // set the consume method as the state's action
           () => {/* actions to perform when exiting the consume state */ }
           );


        State reproduceState = new State("Reproduce",
           () => { Debug.Log("repro");/* actions to perform when entering the consume state */ },
           Reproduce, // set the consume method as the state's action
           () => {/* actions to perform when exiting the consume state */ }
           );


        Transition transitionToPursue = new Transition(
            () => ( fishInfo.Energy < 90),
            () => { },
            pursueState);

        Transition transitionToEvade = new Transition(
           () => GetClosestFish()!=null && GetClosestFish().tag == "Fish",
           () => { },
           evadeState);

        Transition transitionToIdle = new Transition(
            () => (fishInfo.Energy >= 90 && target == null),
            () => { },
            idleState);

        Transition transitionToConsume = new Transition(
            () => (target != null && Vector3.Distance(transform.position, target.position) < 0.2f),
            () => { },
           consumeState);


        Transition transitionToReproduce = new Transition(
            () => (fishInfo.Energy >= 100),
            () => { },
            reproduceState);

        fsm = new StateMachine(idleState);
        pursueState.AddTransition(transitionToIdle);
        pursueState.AddTransition(transitionToConsume);
        pursueState.AddTransition(transitionToReproduce);
        pursueState.AddTransition(transitionToEvade);

        idleState.AddTransition(transitionToIdle);
        idleState.AddTransition(transitionToPursue);
        idleState.AddTransition(transitionToEvade);
        idleState.AddTransition(transitionToReproduce);

        consumeState.AddTransition(transitionToIdle);
        consumeState.AddTransition(transitionToPursue);
        consumeState.AddTransition(transitionToEvade);


        evadeState.AddTransition(transitionToPursue);
        evadeState.AddTransition(transitionToIdle);


        reproduceState.AddTransition(transitionToIdle);
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
            RotateToTarget(away);
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

            if (toClosest != null && target != toClosest && fishInfo.Energy < 75)
            {
                target = toClosest;
            }
        }

        if (fishInfo.FishType != FishType.small)
        {
            // Get vector to closest target
            Transform toClosest = GetClosestFish(false);

            if (toClosest != null && target != toClosest)
            {
                target = toClosest;
            }
        }
        //change maxSpeed and acceleration to chase/flee value
        usingSpeed = fastSpeed;
        away = false;
        MoveForward();
    }


    private void Idle()
    {

        //change maxSpeed and acceleration to idle value
        usingSpeed = 0;
        MoveForward();
    }

    private void Consume()
    {
        Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
        fishInfo.Eat(target);
        Destroy(target.gameObject);
    }

    private void Evade()
    {
        //change maxSpeed and acceleration to chase/flee value
        usingSpeed = fastSpeed;
        away = true;
        MoveForward(true);
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

                    else if (targ.transform.localScale.x < transform.localScale.x) //if not enemy that i'm looking for, then is small than me?
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

        if (best == null) return null;
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
                    best = col; //if the new is greater than the old, change
                }

            }

        }

        if (best == null) return null;
        return best.transform;

    }

    private void MoveForward(bool inverse = false)
    {
        Vector3 move = Vector3.zero;
        if (target != null)
        {
            if (!inverse)
                move = (target.position - transform.position) * usingSpeed * Time.fixedDeltaTime;
            else
                move = (-target.position - transform.position) * usingSpeed * Time.fixedDeltaTime;
        }


        rb.velocity = new Vector3(move.x, move.y, move.z);
        // Limit the maximum speed of the object
        if (rb.velocity.magnitude > usingSpeed)
        {
            rb.velocity = rb.velocity.normalized * usingSpeed;
        }


    }

    private void RotateToTarget(bool _away = false)
    {
        transform.rotation =_away ? Quaternion.LookRotation(target.position + transform.position) :
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
