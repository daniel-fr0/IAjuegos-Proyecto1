using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KinematicController : MonoBehaviour
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
        float dX = Input.GetAxis("Horizontal");
        float dY = Input.GetAxis("Vertical");
        Vector3 input = new Vector3(dX, dY, 0);
        if (input.magnitude > 1)
        {
            input.Normalize();
        }

        character.velocity = input * maxSpeed;
        character.NewOrientation();

        // Double the speed when pressing left shift or R2
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.JoystickButton7))
        {
            character.velocity *= 2;
        }
    }
}
