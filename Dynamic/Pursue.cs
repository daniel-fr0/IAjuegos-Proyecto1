using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursue : MonoBehaviour
{
    private Kinematic character;
    public float maxPrediction = 1.0f;
    // OVERRIDES target in Seek, Seek's target
    // can be accessed with base.target
    public Kinematic target;
    public float targetDebugRadius = 0.25f; // Debugging radius for the target
    public Seek seeker;
    private Kinematic pursueTarget;
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<Kinematic>();

        // Create a target for the Seek behavior
        GameObject targetObject = new GameObject("Pursue Target");
        pursueTarget = targetObject.AddComponent<Kinematic>();
        pursueTarget.position = target.position;

        // Attach target to Seek behavior
        seeker.target = pursueTarget;
    }

    // Update is called once per frame
    void Update()
    {
        // 1. Calculate the target to delegate to seek

        // Work out the distance to target
        Vector3 direction = target.position - character.position;
        float distance = direction.magnitude;

        // Work out our current speed
        float speed = character.velocity.magnitude;

        // Check if speed is too small to give a reasonable prediction time
        float prediction;
        if (speed <= distance / maxPrediction)
        {
            prediction = maxPrediction;
        }
        else
        {
            prediction = distance / speed;
        }

        // Put the target together
        pursueTarget.position = target.position + target.velocity * prediction;

        // 2. Delegate to seek (Seeker component already attached to target)

        // If debugging, draw the radius for the prediction
        if (character.debugInfo)
        {
            character.DrawRadius(seeker.target.position, targetDebugRadius, Color.grey);
        }
    }
}
