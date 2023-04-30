using LibGameAI.FSMs;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

class FishAiBehaviour : MonoBehaviour
{
    [SerializeField] private float normalSpeed, fastSpeed;
    private float usingSpeed = 0;

    private Vector3 target;
    private Transform targetTransform;
    private Fish fishInfo;
    private bool consume = false;

    private bool away = false;
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float tankLenght, tankWidht, tankHeight;

    private StateMachine fsm;
    [SerializeField] private float detectionRadius = 2f;

    private void Awake()
    {
        fishInfo = GetComponent<Fish>();
    }
    private void Start()
    {

        State pursueState = new State("Pursue",
            () => { away = false; Debug.Log("pursue ");/* actions to perform when entering the pursue state */},
            Pursue, // set the Chase method as the state's action
            () => { target = transform.position; }
            );

        State wanderState = new State("Wander",
            () => { away = false; Debug.Log("Wander"); } /* actions to perform when entering the idle state */,
           Wander,
            () => { }
            );


        State evadeState = new State("Evade ",
           () =>
           {    /* actions to perform when entering the evade state */
               away = true; Debug.Log("evade");
           },
           Evade, // set the evade method as the state's action
           () => { target = transform.position; }
           );


        State consumeState = new State("Consume ",
           () =>
           {    /* actions to perform when entering the consume state */
               away = false;
           },
           Consume, // set the consume method as the state's action
           () =>
           {
               consume = false;
               if (targetTransform != null)
                   Destroy(targetTransform.gameObject);
               target = transform.position;
           }
           );


        State reproduceState = new State("Reproduce",
           () => { /* actions to perform when entering the consume state */ },
           Reproduce, // set the consume method as the state's action
           () => {/* actions to perform when exiting the consume state */ }
           );


        Transition transitionToPursue = new Transition(
            () => (
            fishInfo.Energy < 100 &&
            (fishInfo.FishType == FishType.big ?
                        GetClosestFish(false) != null :
                        GetClosestFish(false) != null ||
                        GetClosestAlgae() != null)),
            () => { },
            pursueState);

        Transition transitionToEvade = new Transition(
           () => GetClosestFish() != null,
           () => { },
           evadeState);

        Transition transitionToWander = new Transition(
            () => (
            fishInfo.Energy >= 100 || (
            GetClosestFish() == null &&
            GetClosestFish(false) == null &&
            targetTransform == null &&
            Vector3.Distance(target, transform.position) < 0.4f)),

            () => { },
            wanderState);

        Transition transitionToConsume = new Transition(
            () => (
            target != transform.position &&
            targetTransform != null &&
            consume == true),
            () => { },
           consumeState);


        Transition transitionToReproduce = new Transition(
            () => (fishInfo.Energy >= 100),
            () => { },
            reproduceState);

        fsm = new StateMachine(wanderState);
        pursueState.AddTransition(transitionToWander);
        pursueState.AddTransition(transitionToEvade);
        pursueState.AddTransition(transitionToConsume);
        pursueState.AddTransition(transitionToReproduce);

        wanderState.AddTransition(transitionToWander);
        wanderState.AddTransition(transitionToEvade);
        wanderState.AddTransition(transitionToPursue);
        wanderState.AddTransition(transitionToReproduce);

        consumeState.AddTransition(transitionToWander);
        consumeState.AddTransition(transitionToEvade);
        consumeState.AddTransition(transitionToPursue);
        consumeState.AddTransition(transitionToReproduce);


        evadeState.AddTransition(transitionToEvade);
        evadeState.AddTransition(transitionToPursue);
        evadeState.AddTransition(transitionToWander);


        reproduceState.AddTransition(transitionToWander);
        reproduceState.AddTransition(transitionToEvade);
        reproduceState.AddTransition(transitionToPursue);
    }

    private void Reproduce()
    {
        fishInfo.Reproduce();
    }

    private void FixedUpdate()
    {
        Action actions = fsm.Update();
        actions?.Invoke();
        if (target != Vector3.zero && target != transform.position)
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

            if (toClosest != null && target != toClosest.position && fishInfo.Energy < 75)
            {
                target = toClosest.position;
                targetTransform = toClosest;
            }
        }

        if (fishInfo.FishType != FishType.small)
        {
            // Get vector to closest target
            Transform toClosest = GetClosestFish(false);

            if (toClosest != null && target != toClosest.position)
            {
                target = toClosest.position;
                targetTransform = toClosest;
            }
        }
        //change maxSpeed and acceleration to chase/flee value
        usingSpeed = fastSpeed;
        away = false;
        // Debug.Log("Pursuing " + targetTransform.name);
        MoveForward();
    }


    private void Wander()
    {
        if (Vector3.Distance(transform.position, target) < 0.4f)
        {
            Vector3 pos = new Vector3(
            Random.Range(-tankLenght / 2, tankLenght / 2),
            Random.Range(0, tankHeight),
            Random.Range(-tankWidht / 2, tankWidht / 2));
            target = pos;
        }
        usingSpeed = normalSpeed;
        MoveForward();
    }

    private void Consume()
    {
        fishInfo.Eat(targetTransform);
        target = transform.position;
        targetTransform = null;
        consume = false;
    }

    private void Evade()
    {
        //change maxSpeed and acceleration to chase/flee value
        if (GetClosestFish())
        {
            target = GetClosestFish().position;
            usingSpeed = fastSpeed;
            away = true;
            Debug.Log("Evading " + targetTransform.name);
        }

       
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
        if (target != null && target != transform.position)
        {
            if (!inverse)
                move = (target - transform.position).normalized * usingSpeed * Time.fixedDeltaTime;
            else
                move = (transform.position - target).normalized * usingSpeed * Time.fixedDeltaTime;
        }


        rb.velocity = move;
        // Limit the maximum speed of the object
        if (rb.velocity.magnitude > usingSpeed)
        {
            rb.velocity = rb.velocity.normalized * usingSpeed;
        }


    }

    private void RotateToTarget(bool _away = false)
    {
        Vector3 targetDirection = _away ? (transform.position - target).normalized : (target - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(targetDirection);

    }


    private void Animate()
    {
        if (target == null)
        {
            anim.SetFloat("Speed", 1);
        }
        else if ((Vector3.Distance(target, transform.position) > 0.5f))
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


    void OnTriggerEnter(Collider other)
    {
        // Verifies if the collider is from the target or if is in the target
        if (targetTransform != null)
        {
            if (other.transform == targetTransform ||
                other.transform.parent != null &&
                other.transform.IsChildOf(targetTransform))
            {
                consume = true;
            }
        }

        else consume = false;
    }
}
