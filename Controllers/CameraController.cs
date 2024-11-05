using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Transform target;        // The target to follow
	public Vector3 offset;          // Offset from the target
	public float smoothSpeed = 0.125f; // Smoothness of the camera movement

	public float minZoom = 5f;      // Minimum zoom distance
	public float maxZoom = 15f;     // Maximum zoom distance
	public float zoomSpeed = 10f;    // Speed of zooming

	public float currentZoom = 10f; // Current zoom level

	void Awake()
	{
		// Set the camera's orthographic size to the current zoom level
		Camera.main.orthographicSize = currentZoom;
	}

	void Update()
	{
		// Zoom input from the mouse scroll wheel
		float scrollInput = Input.GetAxis("Mouse ScrollWheel");
		currentZoom -= scrollInput * zoomSpeed;
		currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
	}

	void LateUpdate()
	{
		if (target == null) return;
	
		// Update the camera's orthographic size for zooming
		Camera.main.orthographicSize = currentZoom;
	
		// Calculate the desired position (only X and Y)
		Vector3 desiredPosition = new Vector3(
			target.position.x + offset.x,
			target.position.y + offset.y,
			transform.position.z); // Keep Z position constant
	
		// Smoothly interpolate to the desired position
		Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
	
		// Update the camera's position
		transform.position = smoothedPosition;
	}
}