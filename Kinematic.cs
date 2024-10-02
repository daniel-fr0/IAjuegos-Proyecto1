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
	public bool showInfo = false;

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
			showInfo = !showInfo;
		}

		// Show info (For Debugging)
		if (showInfo)
		{
			// Trace the object's velocity
			Debug.DrawRay(position, velocity, Color.yellow);
		}

    }

	public void ApplySteering(SteeringOutput steering, float maxSpeed)
	{
		velocity += steering.linear * Time.deltaTime;
		rotation += steering.angular * Time.deltaTime;

		if (velocity.magnitude > maxSpeed)
		{
			velocity.Normalize();
			velocity *= maxSpeed;
		}
	}

	public void NewOrientation()
	{
		if (velocity.magnitude > 0)
		{
			orientation = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
		}
	}
}
