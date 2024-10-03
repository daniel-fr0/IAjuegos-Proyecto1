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
        float vX = Mathf.Cos(character.orientation * Mathf.Deg2Rad);
        float vY = Mathf.Sin(character.orientation * Mathf.Deg2Rad);

        // Get velocity from the vector form of the orientation
        character.velocity = new Vector3(vX, vY, 0) * maxSpeed;

        // Change our orientation randomly       
        character.rotation = Random.Range(-maxRotation, maxRotation);
    }
}
