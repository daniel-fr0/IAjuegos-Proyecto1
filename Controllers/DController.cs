using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DynamicController : MonoBehaviour
{
    private Kinematic character;
    public float maxSpeed = 5.0f;
    public float maxAcceleration = 20.0f;
    private float speed;
    private float acceleration;
    public float timeToTarget = 0.1f;
    private SteeringOutput steering;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<Kinematic>();
        steering = new SteeringOutput();
    }

    // Update is called once per frame
    void Update()
    {
        speed = maxSpeed;
        acceleration = maxAcceleration;
        
        Vector3 targetVelocity = getInputVelocity();

        // Target velocity is the input direction times the max speed
        targetVelocity *= speed;

        // Apply steering to the character
        steering.linear = targetVelocity - character.velocity;
        steering.linear /= timeToTarget;
        if (steering.linear.magnitude > acceleration)
        {
            steering.linear.Normalize();
            steering.linear *= acceleration;
        }

        character.ApplySteering(steering, speed);
        character.NewOrientation();
    }

    private Vector3 getInputVelocity()
    {
        // Double the speed when pressing left shift or R2
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.JoystickButton7))
        {
            speed *= 2;
            acceleration *= 2;
        }

        float dX = Input.GetAxis("Horizontal");
        float dY = Input.GetAxis("Vertical");
        Vector3 input = new Vector3(dX, dY, 0);
        if (input.magnitude > 1)
        {
            input.Normalize();
        }

        return input;
    }
}
