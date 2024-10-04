using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : MonoBehaviour
{
	// Parameters for Seek behavior
	private Kinematic character;
	public Kinematic target;
	public float maxAcceleration = 20.0f;
	public float maxSpeed = 5.0f;
	public float fleeRadius = 2.0f;
	public float timeToStop = 0.5f;
	public bool flee = false;

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
		if (!flee)
		{
			steering.linear = target.position - character.position;

			// Full acceleration towards the target
			steering.linear.Normalize();
			steering.linear *= maxAcceleration;
		}
		else
		{
			steering.linear = character.position - target.position;
			// If we are fleeing, we want to stop eventually
			if (steering.linear.magnitude > fleeRadius)
			{
				steering.linear = -character.velocity / timeToStop;

				// Clip the acceleration if it is too fast
				if (steering.linear.magnitude > maxAcceleration)
				{
					steering.linear.Normalize();
					steering.linear *= maxAcceleration;
				}
			}
			else
			{
				// If we are within the flee radius, apply full acceleration
				steering.linear.Normalize();
				steering.linear *= maxAcceleration;
			}
		}

		character.ApplySteering(steering, maxSpeed);
		character.NewOrientation();
	}
}