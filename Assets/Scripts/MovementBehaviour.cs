using System;
using UnityEngine;
using Random = UnityEngine.Random;

class MovementBehaviour : MonoBehaviour
{
    [SerializeField] private float normalSpeed, fastSpeed, lungeSpeed;
    [SerializeField] private Transform target;
    [SerializeField] private float normalAcceleration, lungeAcceleration, fastAcceleration;
    private Ray collisionChecker;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float tankHeight, tankLenght, tankWidht;

    private float usingSpeed = 0, usingAcceleration = 0;

    public bool flee, wander, chase, lunge, idle;

    private void FixedUpdate()
    {

        Chase();


        MoveForward();

    }

    private void Flee()
    {


    }
    private void Wander()
    {

    }
    private void Chase()
    {
        usingSpeed = fastSpeed;
        usingAcceleration = fastAcceleration;

        // Do I have a target?
        if (target != null)
        {
            // Get the direction to the target and rotates 
            RotateToTarget(target.position);

            // Move forward at the given speed and acceleration

        }




    }

    private void Lunge()
    {


    }

    private void MoveForward()
    {
        Vector3 move = (target.position - transform.position) * usingSpeed * Time.fixedDeltaTime;

        rb.velocity = new Vector3(move.x, move.y, move.z);

    }

    private void RotateToTarget(Vector3 targetPos)
    {
        transform.rotation =
            Quaternion.LookRotation(targetPos - transform.position);
    }



}
