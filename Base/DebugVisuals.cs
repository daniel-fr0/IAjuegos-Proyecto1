using UnityEngine;

public class DebugVisuals
{
	public static void DrawRadius(Vector3 center, float radius, Color color, float step = 10)
	{
		// Trace the radius in steps of 'step' degrees
		DrawArc(center, radius, color, 0, 360, step);
	}

	public static void DrawArc(Vector3 center, float radius, Color color, float fromAngle = 0, float toAngle = 360, float step = 10)
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