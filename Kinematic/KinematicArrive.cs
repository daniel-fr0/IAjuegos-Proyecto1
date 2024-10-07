using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicArrive : Seeker
{
    private Kinematic character;
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
        if (!flee)
        {
            // Draw the target radius for debugging
            character.DrawRadius(target.position, targetRadius, Color.cyan);

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
        }
        else
        {
            // Draw the flee radius for debugging
            character.DrawRadius(character.position, fleeRadius, Color.red);

            // Flee while inside the flee radius
            Vector3 direction = character.position - target.position;
            float distance = direction.magnitude;
            if (distance > fleeRadius)
            {
                character.velocity = Vector3.zero;
                return;
            }

            // Slow down as we leave the flee radius in timeToStop seconds
            direction.Normalize();
            character.velocity = direction * (fleeRadius - distance);
            character.velocity /= timeToStop;
        }

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
