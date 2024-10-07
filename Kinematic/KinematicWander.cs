using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicWander : MonoBehaviour
{
    private Kinematic character;
    public float maxSpeed = 1.0f;
    public float maxRotation = 720.0f;
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<Kinematic>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get orientation of character as a vector
        // Orientation is an angle from +X axis
        Vector3 orientation = Kinematic.OrientationAsVector(character.orientation);

        // Get velocity from the vector form of the orientation
        character.velocity = orientation * maxSpeed;

        // Change our orientation randomly       
        character.rotation = Random.Range(-maxRotation, maxRotation);
    }
}
