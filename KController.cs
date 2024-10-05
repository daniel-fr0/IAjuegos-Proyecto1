using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KinematicController : MonoBehaviour
{
    private Kinematic character;
    public float maxSpeed = 5.0f;
    public GameObject[] toggles;
    public GameObject[] scenes;
    public GameObject[] seekers;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<Kinematic>();
    }

    // Update is called once per frame
    void Update()
    {
        float dX = Input.GetAxis("Horizontal");
        float dY = Input.GetAxis("Vertical");
        Vector3 input = new Vector3(dX, dY, 0);
        if (input.magnitude > 1)
        {
            input.Normalize();
        }

        character.velocity = input * maxSpeed;
        character.NewOrientation();

        // Double the speed when pressing left shift or R2
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.JoystickButton7))
        {
            character.velocity *= 2;
        }

        // Toggle on or off game object 1 with Tab key or select button
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.JoystickButton13))
        {
            foreach (GameObject toggle in toggles)
            {
                toggle.SetActive(!toggle.activeSelf);
            }
        }

        // Toggle scenes with Q key or L1
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.JoystickButton4))
        {
            foreach (GameObject scene in scenes)
            {
                scene.SetActive(!scene.activeSelf);
            }
        }

        // Toggle seekers to flee with Space key or X
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            foreach (GameObject seeker in seekers)
            {
                KinematicSeek kinSeek = seeker.GetComponent<KinematicSeek>();
                if (kinSeek != null)
                {
                    kinSeek.flee = !kinSeek.flee;
                }
                else if (seeker.GetComponent<Seek>() != null)
                {
                    Seek seek = seeker.GetComponent<Seek>();
                    seek.flee = !seek.flee;
                }
                else
                {
                    Arrive arrive = seeker.GetComponent<Arrive>();
                    arrive.flee = !arrive.flee;
                }
            }
        }

        // Esc or start button to toggle pause
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton9))
        {
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        }
    }
}
