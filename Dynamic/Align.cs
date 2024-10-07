using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Align : MonoBehaviour
{
    protected Kinematic character;
    public Kinematic target;
    public float maxAngularAcceleration = 2160.0f;
    public float maxRotation = 720.0f;
    // The radius for arriving at the target
    public float targetRadius = 0.1f;
    // The radius for beginning to slow down
    public float slowRadius = 120.0f;
    // The time over which to achieve target speed
    public float timeToTarget = 0.01f;
    protected SteeringOutput steering;
    // Start is called before the first frame update
    protected void Start()
    {
        character = GetComponent<Kinematic>();
        steering = new SteeringOutput();
        character.usingAligner = true;
    }

    // Update is called once per frame
    protected void Update()
    {
        // Draw radiuses for debugging
        if (character.debugInfo)
        {
            character.DrawArc(character.position, character.rotationDebugRadius-0.1f, Color.magenta, character.orientation-slowRadius, character.orientation+slowRadius);
            character.DrawArc(character.position, character.rotationDebugRadius-0.2f, Color.cyan, character.orientation-targetRadius, character.orientation+targetRadius);
        }

        // Get the naive direction to the target
        float rotation = target.orientation - character.orientation;
        
        // Map to range -pi to pi
        rotation = Kinematic.MapToRange(rotation);
        float rotationSize = Mathf.Abs(rotation);

        // Check if we are there
        if (rotationSize < targetRadius)
        {
            return;
        }

        // If we are outside slowRadius, use maximum rotation
        float targetRotation;
        if (rotationSize > slowRadius)
        {
            targetRotation = maxRotation;
        }
        else
        {
            targetRotation = maxRotation * (rotationSize - targetRadius) / (slowRadius - targetRadius);
        }

        // Give direction to the calculated speed
        targetRotation *= rotation / rotationSize;

        // Acceleration tries to get to the target rotation
        steering.angular = targetRotation - character.rotation;
        steering.angular /= timeToTarget;

        // Check if the acceleration is too great
        float angularAcceleration = Mathf.Abs(steering.angular);
        if (angularAcceleration > maxAngularAcceleration)
        {
            steering.angular /= angularAcceleration;
            steering.angular *= maxAngularAcceleration;
        }

        // Update the character's rotation
        character.ApplySteering(steering, maxRotation: maxRotation);
    }
}
