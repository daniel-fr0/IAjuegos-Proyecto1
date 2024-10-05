using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityMatch : MonoBehaviour
{
    protected Kinematic character;
    public Kinematic target;
    public float maxAcceleration = 20.0f;
    public float timeToTarget = 0.1f;
    protected SteeringOutput steering;
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<Kinematic>();
        steering = new SteeringOutput();
    }

    // Update is called once per frame
    void Update()
    {
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
