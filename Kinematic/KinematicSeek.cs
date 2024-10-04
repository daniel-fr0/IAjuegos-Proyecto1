using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicSeek : MonoBehaviour
{
    private Kinematic character;
    public Kinematic target;
    public float maxSpeed = 5.0f;
    public float fleeRadius = 2.0f;
    public bool flee = false;
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
