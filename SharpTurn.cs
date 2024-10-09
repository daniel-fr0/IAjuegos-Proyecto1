using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharpTurn : MonoBehaviour
{
    private Kinematic character;
    private float time = 0.0f;
    public float timeRate = 5.0f;
    public float duration = 1.0f;
    public float acceleration = 20.0f;
    public float angle = 90.0f;
    private float ellapsedTime = 0.0f;
    public bool active = true;
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<Kinematic>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        // Press R or square to enable/disable sharp turn
        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            active = !active;
        }
        if (!active) return;


        // Make a sharp turn every timeRate seconds for duration seconds
        if (time >= timeRate)
        {
            SteeringOutput steering = new SteeringOutput();
            Vector3 direction = Kinematic.OrientationAsVector(character.orientation + angle);
            steering.linear = direction * acceleration;
            character.separationSteering = steering;
            
            ellapsedTime += Time.deltaTime;
            if (ellapsedTime >= duration)
            {
                time = 0.0f;
                ellapsedTime = 0.0f;
            }
        }
    }
}
