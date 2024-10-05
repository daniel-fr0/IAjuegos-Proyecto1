using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityMatch : MonoBehaviour
{
    private Kinematic character;
    public Kinematic target;
    public float maxAcceleration = 20.0f;
    public float timeToTarget = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<Kinematic>();
    }

    // Update is called once per frame
    void Update()
    {
        SteeringOutput steering = new SteeringOutput();

        // Acceleration tries to match target's velocity
        steering.linear = target.velocity - character.velocity;
        steering.linear /= timeToTarget;

        // Check if the acceleration is too fast
        if (steering.linear.magnitude > maxAcceleration)
        {
            steering.linear.Normalize();
            steering.linear *= maxAcceleration;
        }

        // Apply the steering output
        character.ApplySteering(steering);
        character.NewOrientation();
    }
}
