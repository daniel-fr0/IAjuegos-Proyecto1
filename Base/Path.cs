using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Path))]
public class PathEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Path path = (Path)target;

        // Draw the default inspector
        DrawDefaultInspector();

        // Add a button to create the path
        if (GUILayout.Button("Create Path"))
        {
            path.createPath();
            Debug.Log("Path Created!");
        }

        // Add a button to clear the path
        if (GUILayout.Button("Clear Path"))
        {
            path.points = null;
            while (path.transform.childCount > 0)
            {
                DestroyImmediate(path.transform.GetChild(0).gameObject);
            }
            Debug.Log("Path Cleared!");
        }

        // Add a button to reverse the path
        if (GUILayout.Button("Reverse Path"))
        {
            // Reverse the child objects
            int childCount = path.transform.childCount;
            for (int i = 0; i < childCount / 2; i++)
            {
                Transform first = path.transform.GetChild(i);
                Transform second = path.transform.GetChild(childCount - 1 - i);
                Vector3 tempPosition = first.position;
                first.position = second.position;
                second.position = tempPosition;
            }
            Debug.Log("Path Reversed!");
        }
    }
}

public class Path : MonoBehaviour
{
    [Serializable]
    public class polygon
    {
        public int sides;
        public float radius;
        public float startAngle;
        public bool clockWise;
    }
    public polygon basePolygon;
    public bool looped = true;
    public Vector3[] points;
    public bool hideSprite = true;
    public bool debugInfo = false;


    // Start is called before the first frame update
    void Start()
    {
        // If the path is empty, create it
        if (transform.childCount == 0 && (points == null || points.Length == 0))
        {
            createPath();
        }
        else if (points == null || points.Length == 0)
        {
            // Get the points
            points = new Vector3[transform.childCount];
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = transform.GetChild(i).gameObject.transform.position;
            }
        }

        // Hide the sprite renderer if specified
        if (hideSprite && GetComponent<SpriteRenderer>() != null)
        {
            GetComponent<SpriteRenderer>().enabled = false;
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
        if (basePolygon.sides ==  0) return;
        
        float sign = basePolygon.clockWise ? -1 : 1;
        // Create a new path with the given number of points
        points = new Vector3[basePolygon.sides];
        // Create the path based on the number of points and radius
        for (int i = 0; i < points.Length; i++)
        {
            // Calculate the angle of the point
            float angle = basePolygon.startAngle + 2 * Mathf.PI * i / points.Length * sign;

            // Calculate the position of the point
            Vector3 position = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * basePolygon.radius;

            // Create the point
            GameObject point = new GameObject("Point " + i);
            point.transform.parent = transform;
            point.transform.localPosition = position;

            // Add the point to the list
            points[i] = point.transform.position;
        }
    }

    public float getParam(Vector3 position, float lastParam, int maxParamCheck = 0)
    {
        // If a max param check is not provided, check all points
        if (maxParamCheck == 0)
        {
            maxParamCheck = points.Length - 1;
        }

        // Find the closest point on the path to the given position
        int currentPos = (int)lastParam;
        float closestDistance = float.MaxValue;
        float closestParam = lastParam;

        // Start checking from the last point until reaching the max param check
        for (int i = currentPos; i < currentPos + maxParamCheck; i++)
        {
            // Wrap the index around
            int startIndex = i % points.Length;
            int endIndex = (i + 1) % points.Length;
        
            // Get the start and end points of the segment
            Vector3 start = points[startIndex];
            Vector3 end = points[endIndex];

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
                return points[0];
            }
            // If the param is greater than the number of points, return the last point
            if (param > points.Length - 1)
            {
                return points[points.Length - 1];
            }
        }

        // Loop the param to stay in the range [0, points.Length)
        param = Mathf.Repeat(param, points.Length);

        // Regular case
        int startIndex = (int)param;
        int endIndex = (startIndex + 1) % points.Length;

        // Get the start and end points of the segment
        Vector3 start = points[startIndex];
        Vector3 end = points[endIndex];

        // If the segment is too small, return the start point
        if (Vector3.Distance(start, end) < 0.01f)
        {
            return start;
        }

        // Calculate the point on the segment
        float localParam = param - startIndex;
        return Vector3.Lerp(start, end, localParam);
    }

    public void OnDrawGizmos()
    {
        // If in game mode, don't draw path
        if (Application.isPlaying)
        {
            return;
        }

        if (transform.childCount <= 0)
        {
            return;
        }

        for (int i = 0; i < transform.childCount - 1; i++)
        {
            Vector3 start = transform.GetChild(i).position;
            Vector3 end = transform.GetChild(i + 1).position;
            Gizmos.DrawLine(start, end);

            // Add a sphere at the start of the path
            Gizmos.DrawSphere(start, 0.1f);

            // Add a sphere at the end of the path
            if (i == transform.childCount - 2)
            {
                Gizmos.DrawSphere(end, 0.1f);
            }
        }

        // Draw the closing segment if the path is looped
        if (looped)
        {
            Vector3 start = transform.GetChild(transform.childCount - 1).position;
            Vector3 end = transform.GetChild(0).position;
            Gizmos.DrawLine(start, end);
        }
    }

    public void DrawPath()
    {
        for (int i = 0; i < points.Length - 1; i++)
        {
            Vector3 start = points[i];
            Vector3 end = points[i + 1];
            Debug.DrawLine(start, end, Color.gray);
        }
        // Draw the last segment
        if (looped)
        {
            Vector3 start = points[points.Length - 1];
            Vector3 end = points[0];
            Debug.DrawLine(start, end, Color.gray);
        }
    }
}
