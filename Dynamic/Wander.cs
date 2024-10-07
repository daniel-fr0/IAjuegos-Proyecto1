using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : Face
{
    public float wanderOffset = 1.0f;
    public float wanderRadius = 0.25f;
    public float wanderRate = 45.0f;
    public float wanderOrientation = 90.0f;
    public float maxAcceleration = 5.0f;
    public float maxSpeed = 1.0f;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        GameObject targetObject = new GameObject("Wander Target");
        targetObject.transform.parent = transform;
        realTarget = targetObject.AddComponent<Kinematic>();
    }

    // Update is called once per frame
    new void Update()
    {
        // 1. Calculate the target to delegate to face

        // Update the wander orientation
        wanderOrientation += Random.Range(-wanderRate, wanderRate);

        // Calculate the combined target orientation
        float targetOrientation = wanderOrientation + character.orientation;

        // Calculate the center of the wander circle
        Vector3 targetPosition = character.position + wanderOffset * Kinematic.OrientationAsVector(character.orientation);

        // Calculate the target position
        targetPosition += wanderRadius * Kinematic.OrientationAsVector(targetOrientation);

        // 2. Delegate to face
        realTarget.position = targetPosition;
        base.Update();

        // 3. Now set the linear acceleration to be at full
        // acceleration in the direction of the orientation
        steering.linear = maxAcceleration * Kinematic.OrientationAsVector(character.orientation);
        character.ApplySteering(steering, maxSpeed);

        // Debug draw
        if (character.debugInfo)
        {
            // Draw the wander circle
            character.DrawRadius(character.position + wanderOffset * Kinematic.OrientationAsVector(character.orientation), wanderRadius, Color.gray);
            character.DrawRadius(realTarget.position, 0.1f, Color.cyan);
        }
    }
}
