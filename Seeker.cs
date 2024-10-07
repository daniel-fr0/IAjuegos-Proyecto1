using UnityEngine;

public class Seeker: MonoBehaviour
{
	// A component that implements the Flee behavior
	public Kinematic target;
	public bool flee = false;
	public float fleeRadius = 2.0f;
	public float timeToStop = 0.5f;
}