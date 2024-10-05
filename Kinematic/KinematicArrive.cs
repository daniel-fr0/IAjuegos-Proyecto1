using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicArrive : MonoBehaviour
{
    private Kinematic character;
    public Kinematic target;
    public float maxSpeed = 5.0f;
    public float targetRadius = 0.25f;
    public float timeToTarget = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<Kinematic>();
    }

    // Update is called once per frame
    void Update()
    {
        // Draw the target radius for debugging
        character.drawRadius(target.position, targetRadius, Color.cyan);

        // Get the direction to the target
        character.velocity = target.position - character.position;

        // Check if we are there
        if (character.velocity.magnitude < targetRadius)
        {
            character.velocity = Vector3.zero;
            return;
        }

        // We want to reach the target in timeToTarget seconds
        character.velocity /= timeToTarget;


        // If this is too fast, clip it to the max speed
        if (character.velocity.magnitude > maxSpeed)
        {
            character.velocity.Normalize();
            character.velocity *= maxSpeed;
        }

        // Face in the direction we want to move
        character.NewOrientation();
    }
}
