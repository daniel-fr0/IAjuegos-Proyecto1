using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    // Number of points in the path
    public int numPoints;

    // Number of sides in the polygon
    public int polygonSides;
    // Radius of the path
    public float polygonRadius;
    public bool looped = true;
    private GameObject[] points;
    public bool debugInfo = false;


    // Start is called before the first frame update
    void Start()
    {
        // If the path is empty, create it
        if (transform.childCount == 0)
        {
            createPath();
        }
        else
        {
            // Get the number of points
            numPoints = transform.childCount;
            // Get the points
            points = new GameObject[numPoints];
            for (int i = 0; i < numPoints; i++)
            {
                points[i] = transform.GetChild(i).gameObject;
            }
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
        // Create a new path with the given number of points
        numPoints = polygonSides;
        points = new GameObject[numPoints];
        // Create the path based on the number of points and radius
        for (int i = 0; i < numPoints; i++)
        {
            // Calculate the angle of the point
            float angle = 2 * Mathf.PI * i / numPoints;

            // Calculate the position of the point
            Vector3 position = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * polygonRadius;

            // Create the point
            GameObject point = new GameObject("Point " + i);
            point.transform.parent = transform;
            point.transform.localPosition = position;

            // Add the point to the list
            points[i] = point;
        }
    }

    public float getParam(Vector3 position, float lastParam, int maxParamCheck = 0)
    {
        // If a max param check is not provided, check all points
        if (maxParamCheck == 0)
        {
            maxParamCheck = numPoints - 1;
        }

        // Find the closest point on the path to the given position
        int currentPos = (int)lastParam;
        float closestDistance = float.MaxValue;
        float closestParam = lastParam;

        // Start checking from the last point until reaching the max param check
        for (int i = currentPos; i < currentPos + maxParamCheck; i++)
        {
            // Wrap the index around
            int startIndex = i % numPoints;
            int endIndex = (i + 1) % numPoints;
        
            // Get the start and end points of the segment
            Vector3 start = points[startIndex].transform.position;
            Vector3 end = points[endIndex].transform.position;

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
                closestParam = startIndex + param;
            }
        }

        return closestParam;
    }

    public Vector3 getPosition(float param)
    {
        // If the path is not looped
        if (!looped)
        {
            // If the param is less than 0, return the first point
            if (param < 0)
            {
                return points[0].transform.position;
            }
            // If the param is greater than the number of points, return the last point
            if (param > numPoints - 1)
            {
                return points[numPoints - 1].transform.position;
            }
        }

        // Loop the param to stay in the range [0, numPoints)
        param = Mathf.Repeat(param, numPoints);

        // Regular case
        int startIndex = (int)param;
        int endIndex = (startIndex + 1) % numPoints;

        // Get the start and end points of the segment
        Vector3 start = points[startIndex].transform.position;
        Vector3 end = points[endIndex].transform.position;

        // If the segment is too small, return the start point
        if (Vector3.Distance(start, end) < 0.01f)
        {
            return start;
        }

        // Calculate the point on the segment
        float localParam = param - startIndex;
        return Vector3.Lerp(start, end, localParam);
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

        if (numPoints <= 0)
        {
            return;
        }

        for (int i = 0; i < numPoints - 1; i++)
        {
            Vector3 start = transform.GetChild(i).position;
            Vector3 end = transform.GetChild(i + 1).position;
            Gizmos.DrawLine(start, end);

            // Add a sphere at the start of the path
            Gizmos.DrawSphere(start, 0.1f);
        }
        // Draw the last segment
        if (looped)
        {
            Vector3 start = transform.GetChild(numPoints - 1).position;
            Vector3 end = transform.GetChild(0).position;
            Gizmos.DrawLine(start, end);

            // Add a sphere at the start of the path
            Gizmos.DrawSphere(start, 0.1f);
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
