using UnityEngine;

public class SteeringOutput
{
	public Vector3 linear;
	public float angular;

	public SteeringOutput()
	{
		linear = Vector3.zero;
		angular = 0.0f;
	}
}