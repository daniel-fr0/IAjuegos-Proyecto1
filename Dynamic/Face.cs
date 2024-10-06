using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : MonoBehaviour
{
    private Kinematic character;
    public Kinematic target;
    public Align aligner;
    private Kinematic faceTarget;
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<Kinematic>();

        // Create a target for the Align behavior
        GameObject targetObject = new GameObject("Face Target");
        faceTarget = targetObject.AddComponent<Kinematic>();
        aligner.target = faceTarget;
    }

    // Update is called once per frame
    void Update()
    {
        // 1. Calculate the target to delegate to align

        // Work out the direction to target
        Vector3 direction = target.position - character.position;

        // Check for a zero direction, and make no change if so
        if (direction.magnitude == 0)
        {
            faceTarget.orientation = target.orientation;
            return;
        }

        // 2. Delegate to align (Align component already attached to target)
        faceTarget.orientation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // If debugging, draw the direction of the target
        if (character.debugInfo)
        {
            Debug.DrawLine(character.position, direction.normalized * character.rotationDebugRadius, Color.cyan);
        }
    }
}
