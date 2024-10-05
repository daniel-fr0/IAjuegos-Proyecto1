using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrive : MonoBehaviour
{
    // Parameters for Arrive behavior
    protected Kinematic character;
    public Kinematic target;
    public float maxAcceleration = 20.0f;
    public float maxSpeed = 5.0f;
    public float targetRadius = 0.25f;
    public float slowRadius = 3.0f;
    public float timeToTarget = 0.1f;
    protected SteeringOutput steering;

    // Parameters for Flee behavior
    public float fleeRadius = 2.0f;
    public float timeToStop = 0.5f;
    public bool flee = false;
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<Kinematic>();
        steering = new SteeringOutput();
    }

    // Update is called once per frame
    void Update()
    {
        if (!flee)
        {
            arriveUpdate();
        }
        else
        {
            fleeUpdate();
        }

        // Update the character speed and orientation
        character.ApplySteering(steering, maxSpeed);
        character.NewOrientation();
    }

    void arriveUpdate() {
        // Draw the target+slow radius for debugging
        character.DrawRadius(target.position, targetRadius, Color.cyan);
        character.DrawRadius(target.position, slowRadius, Color.magenta);

        // Get the direction to the target
        Vector3 direction = target.position - character.position;
        float distance = direction.magnitude;

        // Check if we are there
        if (distance < targetRadius)
        {
            return;
        }

        // If we are outside the slowRadius, then go max speed
        float targetSpeed;
        if (distance > slowRadius)
        {
            targetSpeed = maxSpeed;
        }
        else
        {
            targetSpeed = maxSpeed * (distance - targetRadius) / (slowRadius - targetRadius);
        }

        // The target velocity combines speed and direction
        Vector3 targetVelocity = direction;
        targetVelocity.Normalize();
        targetVelocity *= targetSpeed;

        // Acceleration tries to get to the target velocity
        steering.linear = targetVelocity - character.velocity;
        steering.linear /= timeToTarget;

        // Check if the acceleration is too fast
        if (steering.linear.magnitude > maxAcceleration)
        {
            steering.linear.Normalize();
            steering.linear *= maxAcceleration;
        }
    }

    void fleeUpdate() {
        // Draw the flee radius for debugging
        character.DrawRadius(character.position, fleeRadius, Color.red);
        
        Vector3 direction = character.position - target.position;
        float distance = direction.magnitude;

        // If we are fleeing, we want to stop eventually
        if (distance > fleeRadius)
        {
            steering.linear = -character.velocity / timeToStop;

            // Clip the acceleration if it is too fast
            if (steering.linear.magnitude > maxAcceleration)
            {
                steering.linear.Normalize();
                steering.linear *= maxAcceleration;
            }
            return;
        }

        float targetSpeed = maxSpeed * fleeRadius / distance;

        // The target velocity combines speed and direction
        Vector3 targetVelocity = direction;
        targetVelocity.Normalize();
        targetVelocity *= targetSpeed;

        // Acceleration tries to get to the target velocity
        steering.linear = targetVelocity - character.velocity;
        steering.linear /= timeToStop;

        // Check if the acceleration is too fast
        if (steering.linear.magnitude > maxAcceleration)
        {
            steering.linear.Normalize();
            steering.linear *= maxAcceleration;
        }
    }
}
