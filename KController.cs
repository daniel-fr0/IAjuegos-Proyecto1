using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KinematicController : MonoBehaviour
{
    private Kinematic character;
    public float maxSpeed = 5.0f;
    private int scene = 0;
    public GameObject[] scenes;
    public GameObject[] toggleShow;
    public Flee[] toggleFlee;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<Kinematic>();

        // Set the first scene to active
        scenes[scene].SetActive(true);
        // Set the rest of the scenes to inactive
        for (int i = 1; i < scenes.Length; i++)
        {
            scenes[i].SetActive(false);
        }
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

        // Go to next scene with E key or R1
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton5))
        {
            scenes[scene].SetActive(false);
            scene = (scene + 1) % scenes.Length;
            scenes[scene].SetActive(true);
        }

        // Go to previous scene with Q key or L1
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.JoystickButton4))
        {
            scenes[scene].SetActive(false);
            scene = (scene - 1 + scenes.Length) % scenes.Length;
            scenes[scene].SetActive(true);
        }

        // Toggle on or off game objects with Tab key or select button
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.JoystickButton13))
        {
            foreach (GameObject toggle in toggleShow)
            {
                toggle.SetActive(!toggle.activeSelf);
            }
        }

        // Toggle game objects to flee with Space key or X
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            foreach (Flee entity in toggleFlee)
            {
                entity.flee = !entity.flee;
            }
        }

        // Esc or start button to toggle pause
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton9))
        {
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        }
    }
}
