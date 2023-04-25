
using System.IO;
using System;
using UnityEngine;

class MovementBehaviour : Monobehaviour
{
    private float FleeSpeed, WanderSpeed, chaseSpeed, lungeSpeed;
    private Vector3 target;
    private float normalAcceleration, lungeAcceleration,fastAcceleration;
    private Ray collisionChecker;
    private Rigidbody rb;
    
    
    private float usingSpeed,usingAcceleration;

    void Update
    {
    }

    private void Flee()
    {
          
            // Do I have a target?
            if (target != null)
            {
                // Get the direction to the target and rotates 
                // the fish to the other side
               vector3 fleeTarget = target.position*-1;
               
               RotateToTarget(fleeTarget);
               
                // Give full acceleration along this direction
                Move();

            }

    }
    private void Wander()
    {

    }
    private void Chase()
    {

    }

    private void Lunge()
    {
        usingSpeed = lungeSpeed;
        usingAcceleration = lungeAcceleration;
        Chase();

    }

    private void Idle()
    {   
        
    }

    private void MoveForward()
    {
        Vector3 move = transform.position * Vector3.forward *
                        usingSpeed* usingAcceleration*Time.deltaTime;
                        
        rb.velocity = new vector3(rb.velocity.x, rb.velocity.y,move.z );
    
    }

    private void RotateToTarget(Vector3 targetPos)
    {
        transform.rotation = 
            Quaternion.LookRotation(targetPos - transform.position);
    }



}
