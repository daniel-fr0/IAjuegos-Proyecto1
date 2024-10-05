using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Kinematic : MonoBehaviour
{
    // Kinematic properties
    public Vector3 position;
	public float orientation;
	public Vector3 velocity;
	public float rotation;
	public float speed; // Debugging purposes only
	public bool debugInfo = false;

    // Start is called before the first frame update
    void Start()
    {
    	// Get the object's position and rotation
		position = transform.position;
		orientation = transform.rotation.eulerAngles.z;   
    }

    // Update is called once per frame
    void Update()
    {
        // Update kinematic properties
		position += velocity * Time.deltaTime;
		orientation += rotation * Time.deltaTime;

		// Update object's position and rotation
		transform.position = position;
		transform.rotation = Quaternion.Euler(0, 0, orientation);

		// Record speed (For Debugging)
		speed = velocity.magnitude;

		// Toggle info with F key OR triangle button
		if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton3))
		{
			debugInfo = !debugInfo;
		}

		// Show info (For Debugging)
		if (debugInfo)
		{
			// Trace the object's velocity
			Debug.DrawRay(position, velocity, Color.yellow);
		}

    }

	public void ApplySteering(SteeringOutput steering, float maxSpeed = float.MaxValue, float maxRotation = float.MaxValue)
	{
		velocity += steering.linear * Time.deltaTime;
		rotation += steering.angular * Time.deltaTime;

		if (velocity.magnitude > maxSpeed)
		{
			velocity.Normalize();
			velocity *= maxSpeed;
		}

		if (Mathf.Abs(rotation) > maxRotation)
		{
			rotation = Mathf.Sign(rotation) * maxRotation;
		}
	}

	public void NewOrientation()
	{
		if (velocity.magnitude > 0)
		{
			orientation = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
		}
	}

	public void DrawRadius(Vector3 center, float radius, Color color, float step = 10)
	{
		if (debugInfo)
		{
			// Trace the radius in steps of 'step' degrees
			for (int i = 0; i < 360/step; i++)
			{
				float angleStart = i * step * Mathf.Deg2Rad;
				float angleEnd = (i + 1) * step * Mathf.Deg2Rad;
				Vector3 start = center + new Vector3(Mathf.Cos(angleStart), Mathf.Sin(angleStart), 0) * radius;
				Vector3 end = center + new Vector3(Mathf.Cos(angleEnd), Mathf.Sin(angleEnd), 0) * radius;
				Debug.DrawLine(start, end, color);
			}
		}
	}

	/// <summary>
	/// Maps an angle to the range [-180, 180]
	/// </summary>
	/// <param name="angle">An angle in degrees not bound</param>
	/// <returns>An angle in degrees between [-180, 180]</returns>
	public static float MapToRange(float angle)
	{
		// This needs to use real clock arithmetic
		float res = angle + 180;

		res = (res % 360 + 360) % 360; // This is the same as "res mod 360"

		return res - 180;
	}
}
