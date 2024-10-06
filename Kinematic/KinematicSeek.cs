using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicSeek : Flee
{
    private Kinematic character;
    public float maxSpeed = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<Kinematic>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get the direction to the target
        if (flee)
        {
            // Draw the flee radius for debugging
            character.DrawRadius(character.position, fleeRadius, Color.red);

            // Flee while inside the flee radius
            character.velocity = character.position - target.position;
            if (character.velocity.magnitude > fleeRadius)
            {
                character.velocity = Vector3.zero;
                return;
            }
        }
        else
            character.velocity = target.position - character.position;
            

        // The velocity is along this direction, at full speed
        character.velocity.Normalize();
        character.velocity *= maxSpeed;

        // Face in the direction we want to move
        character.NewOrientation();
    }
}
