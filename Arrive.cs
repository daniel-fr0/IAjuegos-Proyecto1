using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrive : MonoBehaviour
{
    // Parameters for Arrive behavior
    private Kinematic character;
    public Kinematic target;
    public float maxAcceleration = 20.0f;
    public float maxSpeed = 5.0f;
    public float targetRadius = 0.25f;
    public float slowRadius = 3.0f;
    public float timeToTarget = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<Kinematic>();
    }

    // Update is called once per frame
    void Update()
    {
        // Draw the target+slow radius for debugging
        character.drawRadius(target.position, targetRadius, Color.cyan);
        character.drawRadius(target.position, slowRadius, Color.magenta);

        SteeringOutput steering = new SteeringOutput();

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

        // Update the character speed and orientation
        character.ApplySteering(steering, maxSpeed);
        character.NewOrientation();
    }
}
