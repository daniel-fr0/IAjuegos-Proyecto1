using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Separation : MonoBehaviour
{
	private Kinematic character;
    // Separation properties
	public float maxAcceleration = 10.0f;
	// The threshold distance for separation
	public float threshold = 0.5f;
	// The constatn coefficient fo decay for the inverse square law
	public float decayCoefficient = 0.5f;

	// List of potential targets
	static public List<Kinematic> targets = new List<Kinematic>();

	// Toggle separation
	public bool active = true;

    // Start is called before the first frame update
    void Start()
    {
		// Every object is a target
		character = GetComponent<Kinematic>();
		targets.Add(character);
    }

    // Update is called once per frame
    void Update()
    {
		if (!active) return;

		// Calcualte the separation steering
		SteeringOutput result = new SteeringOutput();

		// Loop through all targets
		foreach (Kinematic target in targets)
		{
			// Check if the target is not the current object
			if (target == character) continue;

			// Check if the target is close
			Vector3 direction = character.position - target.position;
			float distance = direction.magnitude;

			if (distance < threshold)
			{
				// Calculate the strength of the separation
				// (here using the inverse square law)
				float strength = Mathf.Min(decayCoefficient / (distance * distance), maxAcceleration);

				// Add the acceleration to the steering
				result.linear += direction.normalized * strength;
			}
		}

		// Clip acceleration to max acceleration
		if (result.linear.magnitude > maxAcceleration)
		{
			result.linear = result.linear.normalized * maxAcceleration;
		}

		// Add the separation steering to the character
		character.separationSteering = result;

		// Debugging purposes only
		if (character.debugInfo)
		{
			// Scale acceleration to draw within the threshold radius
			Vector3 linear = character.separationSteering.linear;
			float magnitude = linear.magnitude / maxAcceleration * threshold;
			linear = linear.normalized * magnitude;


			Debug.DrawRay(character.position, linear, Color.red);
			character.DrawRadius(character.position, threshold, Color.red);
		}
    }
}
