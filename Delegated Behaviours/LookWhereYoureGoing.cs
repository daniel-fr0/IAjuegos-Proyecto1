using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookWhereYoureGoing : Align
{
    private Kinematic faceTarget;
    public bool showTarget = true;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        // Create a target for the Align behavior
        GameObject targetObject = new GameObject("LWYK Target");
        targetObject.transform.parent = transform;
        faceTarget = targetObject.AddComponent<Kinematic>();
        target = faceTarget;
    }

    // Update is called once per frame
    new void Update()
    {
        // 1. Calculate the target to delegate to align

        Vector3 crossHairDirection; // For debugging

        // Check for a zero direction, and make no change if so
        if (character.velocity.magnitude <= 0.01)
            // For debugging, crosshair direction
            crossHairDirection = Kinematic.OrientationAsVector(faceTarget.orientation);
        else
        {
            // Otherwise set the target based on the velocity
            faceTarget.orientation = Mathf.Atan2(character.velocity.y, character.velocity.x) * Mathf.Rad2Deg;

            // For debugging, crosshair direction
            crossHairDirection = character.velocity.normalized;
        }
        
        // 2. Delegate to align (Align component already attached to target)
        base.Update();

        // If debugging, draw the target (crosshair)
        if (character.debugInfo && showTarget)
        {
            Debug.DrawLine(
                character.position + crossHairDirection * (character.rotationDebugRadius-0.1f),
                character.position + crossHairDirection * character.rotationDebugRadius,
                Color.cyan
            );

            character.DrawArc(
                character.position,
                character.rotationDebugRadius - 0.05f,
                Color.cyan,
                faceTarget.orientation - 5,
                faceTarget.orientation + 5
            );
        }
    }
}
