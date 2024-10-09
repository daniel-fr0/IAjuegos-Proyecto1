using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : Align
{
    public Kinematic realTarget;
    // Start is called before the first frame update
    new protected void Start()
    {
        base.Start();

        // Create a target for the Align behavior
        GameObject targetObject = new GameObject("Face Target");
        targetObject.transform.parent = transform;
        target = targetObject.AddComponent<Kinematic>();
    }

    // Update is called once per frame
    new protected void Update()
    {
        // 1. Calculate the target to delegate to align

        // Work out the direction to target
        Vector3 direction = realTarget.position - character.position;

        // Check for a zero direction, and make no change if so
        if (direction.magnitude == 0)
        {
            target.orientation = realTarget.orientation;
            base.Update();
            return;
        }

        // 2. Delegate to align (Align component already attached to target)
        target.orientation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        base.Update();

        // If debugging, draw the target (crosshair)
        if (character.debugInfo)
        {
            Debug.DrawLine(
                character.position + direction.normalized * (character.rotationDebugRadius-0.1f),
                character.position + direction.normalized * character.rotationDebugRadius,
                Color.cyan
            );

            character.DrawArc(
                character.position,
                character.rotationDebugRadius - 0.05f,
                Color.cyan,
                target.orientation - 5,
                target.orientation + 5
            );
        }
    }
}
