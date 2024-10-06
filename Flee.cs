using UnityEngine;

public class Flee: MonoBehaviour
{
	// A component that implementas the Flee behavior
	// should have a toggle to enable/disable the behavior
	public bool flee = false;
	public float fleeRadius = 2.0f;
	public float timeToStop = 0.5f;
}