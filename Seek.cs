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
		steering.linear = target.position - character.position;

		// Full acceleration towards the target
		steering.linear.Normalize();
		steering.linear *= maxAcceleration;

		character.ApplySteering(steering, maxSpeed);
		character.NewOrientation();
	}
}