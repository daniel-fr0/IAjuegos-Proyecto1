using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    // Number of vertices in the path
    public int numPoints;
    // Radius of the path
    public float radius;
    public bool looped = true;
    public GameObject[] points;
    public bool debugInfo = false;


    // Start is called before the first frame update
    void Start()
    {
        // If the path is empty, create it
        if (points.Length == 0)
        {
            createPath();
        }
        else
        {
            numPoints = points.Length;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If the F key or triangle button is pressed, toggle debug info
        if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            debugInfo = !debugInfo;
        }

        if (debugInfo)
        {
            DrawPath();
        }
    }

    public void createPath()
    {
        points = new GameObject[numPoints];
        // Create the path based on the number of points and radius
        for (int i = 0; i < numPoints; i++)
        {
            float angle = 2 * Mathf.PI * i / numPoints;
            Vector3 position = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            GameObject point = new GameObject("Point" + i);
            point.transform.parent = transform;
            point.transform.localPosition = position;
            points[i] = point;
        }
    }

    public float getParam(Vector3 position, float lastParam, int maxParamCheck = -1)
    {
        // If a max param check is not provided, check all points
        if (maxParamCheck == -1)
        {
            maxParamCheck = numPoints;
        }

        // Find the closest point on the path to the given position
        float closestDistance = float.MaxValue;
        float closestParam = lastParam;

        // Start checking from the last point until reaching the max param check
        for (int i = (int)lastParam; i < (int)lastParam + maxParamCheck - 1; i++)
        {
            // Get the start and end points of the segment
            Vector3 start = points[i].transform.position;
            Vector3 end = points[(i + 1) % numPoints].transform.position;

            // Find the closest point on the segment
            Vector3 closest;
            float param;

            // If the segment is too small, use the start point
            if (Vector3.Distance(start, end) < 0.01f)
            {
                param = 0;
                closest = start;
            }

            // Calculate the closest point on the segment
            else
            {
                param = Vector3.Dot(position - start, end - start) / Vector3.Dot(end - start, end - start);
                param = Mathf.Clamp(param, 0.0f, 1.0f);
                closest = start + param * (end - start);
            }

            // Check if the closest point is the closest so far
            float distance = Vector3.Distance(position, closest);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestParam = i + param;
            }
        }

        return closestParam;
    }

    public Vector3 getPosition(float param)
    {

        // If the path is looped, wrap the parameter
        if (looped)
        {
            param = param % (numPoints - 1);
        }

        // Otherwise, clamp the parameter so it is within the path
        // it will specify at most the last point
        else
        {
            param = Mathf.Clamp(param, 0.0f, numPoints - 1);
        }

        // Get the start and end points of the segment
        int index = (int)param;
        Vector3 start = transform.GetChild(index).position;
        Vector3 end = transform.GetChild((index + 1) % numPoints).position;

        // Interpolate between the points
        float t = param - index;
        return Vector3.Lerp(start, end, t);
    }

    public void reversePath()
    {
        // Reverse the order of the points
        for (int i = 0; i < numPoints / 2; i++)
        {
            GameObject temp = points[i];
            points[i] = points[numPoints - i - 1];
            points[numPoints - i - 1] = temp;
        }
    }

    public void OnDrawGizmos()
    {
        // If in game mode, don't draw path
        if (Application.isPlaying)
        {
            return;
        }

        for (int i = 0; i < numPoints - 1; i++)
        {
            Vector3 start = transform.GetChild(i).position;
            Vector3 end = transform.GetChild(i + 1).position;
            Gizmos.DrawLine(start, end);
        }
        // Draw the last segment
        if (looped)
        {
            Vector3 start = transform.GetChild(numPoints - 1).position;
            Vector3 end = transform.GetChild(0).position;
            Gizmos.DrawLine(start, end);
        }
    }

    public void DrawPath()
    {
        for (int i = 0; i < numPoints - 1; i++)
        {
            Vector3 start = transform.GetChild(i).position;
            Vector3 end = transform.GetChild(i + 1).position;
            Debug.DrawLine(start, end, Color.gray);
        }
        // Draw the last segment
        if (looped)
        {
            Vector3 start = transform.GetChild(numPoints - 1).position;
            Vector3 end = transform.GetChild(0).position;
            Debug.DrawLine(start, end, Color.gray);
        }
    }
}
