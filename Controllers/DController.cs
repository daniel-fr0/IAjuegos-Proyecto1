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
    private SteeringOutput steering;

    #region Input System
    public Vector3 input;
    private InputSystem_Actions controls;

    void Awake()
    {
        controls = new InputSystem_Actions();

        controls.Player.Move.performed += ctx => input = ctx.ReadValue<Vector2>();
        controls.Player.Sprint.performed += ctx => UpdateSpeed(ctx.ReadValue<float>());
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void UpdateSpeed(float sprint)
    {
        if (sprint > 0)
        {
            speed = maxSpeed * 2;
            acceleration = maxAcceleration * 2;
        }
        else
        {
            speed = maxSpeed;
            acceleration = maxAcceleration;
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<Kinematic>();
        steering = new SteeringOutput();
        speed = maxSpeed;
        acceleration = maxAcceleration;
    }

    // Update is called once per frame
    void Update()
    {
        // Target velocity is the input direction times the max speed
        Vector3 targetVelocity = input * speed;

        // Apply steering to the character
        steering.linear = targetVelocity - character.velocity;
        if (steering.linear.magnitude > acceleration * Time.deltaTime)
        {
            steering.linear.Normalize();
            steering.linear *= acceleration * Time.deltaTime;
        }
        steering.linear /= Time.deltaTime;

        character.ApplySteering(steering, speed);
        character.NewOrientation();
    }
}