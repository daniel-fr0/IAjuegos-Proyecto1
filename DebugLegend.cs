using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLegend : MonoBehaviour
{
    public GameObject velocity;
    public GameObject rotation;
    public GameObject acceleration;
    public GameObject slowRadius;
    public GameObject targetRadius;
    public GameObject path;
    public GameObject threshold;

    private Kinematic kinematic;

    // Start is called before the first frame update
    void Start()
    {
        kinematic = GetComponent<Kinematic>();
        kinematic.debugInfo = true;
    }

    // Update is called once per frame
    void Update()
    {

        Debug.DrawLine(velocity.transform.position, velocity.transform.position + Vector3.right, Color.yellow);

        kinematic.DrawArc(rotation.transform.position, 0.3f, Color.green, 0.0f, 90.0f);
        kinematic.DrawArc(rotation.transform.position, 0.3f, Color.red, 270.0f, 360.0f);

        Debug.DrawLine(acceleration.transform.position, acceleration.transform.position + Vector3.right, Color.red);

        kinematic.DrawRadius(slowRadius.transform.position, 0.3f, Color.blue);
        kinematic.DrawArc(slowRadius.transform.position, 0.35f, Color.magenta, 270.0f, 450.0f);

        kinematic.DrawRadius(targetRadius.transform.position, 0.15f, Color.cyan);
        Debug.DrawLine(
            targetRadius.transform.position + Vector3.right * 0.2f,
            targetRadius.transform.position + Vector3.right * 0.25f,
            Color.cyan
        );

        kinematic.DrawArc(
            targetRadius.transform.position,
            0.225f,
            Color.cyan,
            355.0f,
            365.0f
        );

        kinematic.DrawRadius(path.transform.position, 0.3f, Color.gray);

        kinematic.DrawRadius(threshold.transform.position, 0.3f, Color.red);
    }
}
