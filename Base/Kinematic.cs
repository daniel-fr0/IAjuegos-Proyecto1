using UnityEngine;
using System;

public class Kinematic : MonoBehaviour
{
    // Kinematic properties
    public Vector3 position;
	public float orientation;
	public Vector3 velocity;
	public float rotation;
	public float speed; // Debugging purposes only
	public bool debugInfo = false;
	public float rotationDebugRadius = 0.5f;
	public bool usingAligner = false;
	public float accelerationDebugScale = 0.1f;
	public SteeringOutput separationSteering = new SteeringOutput();
	// Constraints like freeze position or rotation
	public bool freezePosition = false;
	public bool freezeRotation = false;
	public bool freezeSpeed = false;
	public bool avoidCollisions = false;
	[Serializable]
	public class AvoidCollisionsSettings
	{
		public float avoidDistance = 0.5f;
		public float lookAhead = 0.25f;
		public float avoidForce = 100.0f;
		[HideInInspector]
		public SteeringOutput steering = new SteeringOutput();
		[HideInInspector]
		public bool collisionDetected = false;
	}
	public AvoidCollisionsSettings avoidCollisionsSettings = new AvoidCollisionsSettings();

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
		if (avoidCollisions) DetectCollisions();

        // Update kinematic properties
		position += velocity * Time.deltaTime;
		orientation += rotation * Time.deltaTime;

		// Update object's position and rotation
		if (!freezePosition) transform.position = position;
		if (!freezeRotation) transform.rotation = Quaternion.Euler(0, 0, orientation);

		// Record speed (For Debugging)
		if (!freezeSpeed) speed = velocity.magnitude;
		else speed = 0;

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
			// Trace the object's rotation
			DrawRotation(Color.green, Color.red);
		}

    }

	public void ApplySteering(SteeringOutput steering, float maxSpeed = float.MaxValue, float maxRotation = float.MaxValue)
	{
		// If there is a collision, use the collision avoidance steering
		if (avoidCollisions && avoidCollisionsSettings.collisionDetected)
		{
			steering = avoidCollisionsSettings.steering;
		}

		// Add extra steering when using separation
		steering.linear += separationSteering.linear;
		separationSteering = new SteeringOutput();

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

		// If debug, draw acceleration
		if (debugInfo)
		{
			Debug.DrawRay(position, steering.linear * accelerationDebugScale, Color.red);
		}
	}

	private void DetectCollisions()
	{
		// Check for collisions
		RaycastHit2D hit = Physics2D.Raycast(position, velocity.normalized, avoidCollisionsSettings.lookAhead);

		// If a collision is detected, avoid it
		if (hit.collider != null)
		{
			// Calculate "seek" target position, the idea is to "delegate" the steering to the Seeker behaviour
			Vector3 targetPosition = hit.point + hit.normal * avoidCollisionsSettings.avoidDistance;

			// Seeker behaviour is to calculate the steering output based on the target position
			avoidCollisionsSettings.steering.linear = targetPosition - position;

			// Steer towards the target at current speed
			avoidCollisionsSettings.steering.linear.Normalize();
			avoidCollisionsSettings.steering.linear *= avoidCollisionsSettings.avoidForce;

			avoidCollisionsSettings.collisionDetected = true;
			ApplySteering(avoidCollisionsSettings.steering);
		}
		else
		{
			avoidCollisionsSettings.collisionDetected = false;
		}
	}

	public void CopyFrom(Kinematic other)
	{
		position = other.position;
		orientation = other.orientation;
		velocity = other.velocity;
		rotation = other.rotation;
		speed = other.speed;
		debugInfo = other.debugInfo;
		rotationDebugRadius = other.rotationDebugRadius;
		usingAligner = other.usingAligner;
		accelerationDebugScale = other.accelerationDebugScale;
		separationSteering = other.separationSteering;
		freezePosition = other.freezePosition;
		freezeRotation = other.freezeRotation;
		freezeSpeed = other.freezeSpeed;
		avoidCollisions = other.avoidCollisions;
		avoidCollisionsSettings = other.avoidCollisionsSettings;
	}

	public void NewOrientation()
	{
		// Do not update orientation if object is using an align behavior
		if (usingAligner) return;

		if (velocity.magnitude > 0)
		{
			orientation = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
		}
	}

	private void DrawRotation(Color positive, Color negative, float step = 10)
	{
		if (rotation > 0)
		{
			DrawArc(position, rotationDebugRadius, positive, orientation, orientation+rotation, step);
		}
		else if (rotation < 0)
		{
			DrawArc(position, rotationDebugRadius, negative, orientation+rotation, orientation, step);
		}
	}

	public void DrawRadius(Vector3 center, float radius, Color color, float step = 10)
	{
		if (debugInfo)
		{
			// Trace the radius in steps of 'step' degrees
			DrawArc(center, radius, color, 0, 360, step);
		}
	}

	public void DrawArc(Vector3 center, float radius, Color color, float fromAngle = 0, float toAngle = 360, float step = 10)
	{
		if (debugInfo)
		{
			// Trace the arc in steps of 'step' degrees
			int steps = (int)((toAngle - fromAngle) / step);
			for (int i = 0; i < steps; i ++)
			{
				float angleStart = (fromAngle + i * step) * Mathf.Deg2Rad;
				float angleEnd = angleStart + step * Mathf.Deg2Rad;
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

	/// <summary>
	/// Returns a vector with the orientation as angle.
	/// The angle is in degrees from the x-axis.
	/// </summary>
	/// <param name="orientation">The orientation in degrees</param>
	/// <returns>A vector with the orientation as angle</returns>
	public static Vector3 OrientationAsVector(float orientation)
	{
		return new Vector3(Mathf.Cos(orientation * Mathf.Deg2Rad), Mathf.Sin(orientation * Mathf.Deg2Rad), 0);
	}
}
