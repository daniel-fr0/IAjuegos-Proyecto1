using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public int scene = 0;
    public GameObject[] scenes;
    public GameObject[] toggleShow;
    public Seeker[] toggleFlee;
    
    // Pause system variables
    private bool isPaused = false;
    private GUIStyle buttonStyle;
    private GUIStyle labelStyle;
    public int buttonFontSize = 20;
    public int labelFontSize = 30;

    // Start is called before the first frame update
    void Start()
    {
        if (scenes.Length == 0) return;
        
        // Set right scene active
        for (int i = 0; i < scenes.Length; i++)
        {
            scenes[i].SetActive(i == scene);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Go to next scene with E key or R1
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton5))
        {
            if (scenes.Length == 0) return;
            scenes[scene].SetActive(false);
            scene = (scene + 1) % scenes.Length;
            scenes[scene].SetActive(true);
        }

        // Go to previous scene with Q key or L1
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.JoystickButton4))
        {
            if (scenes.Length == 0) return;
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
            foreach (Seeker entity in toggleFlee)
            {
                entity.flee = !entity.flee;
            }
        }

        // Toggle game objects to separate with Left Control key or circle button
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            // Look for all separation scripts
            Separation[] separations = FindObjectsByType<Separation>(FindObjectsSortMode.None);
            foreach (Separation separation in separations)
            {
                separation.active = !separation.active;
            }
        }

        // Esc or start button to toggle pause
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton9))
        {
            TogglePause();
        }
    }

    void OnGUI()
    {
        // Only show pause menu if the game is paused
        if (isPaused)
        {
            // Initialize styles if they haven't been created yet
            if (buttonStyle == null)
            {
                buttonStyle = new GUIStyle(GUI.skin.button);
                buttonStyle.fontSize = buttonFontSize;
                buttonStyle.normal.textColor = Color.black;
                buttonStyle.hover.textColor = Color.gray;
            }

            if (labelStyle == null)
            {
                labelStyle = new GUIStyle(GUI.skin.label);
                labelStyle.fontSize = labelFontSize;
                labelStyle.normal.textColor = Color.white;
                labelStyle.alignment = TextAnchor.MiddleCenter;
            }

            // Semi-transparent black background
            GUI.color = new Color(0, 0, 0, 0.8f);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
            GUI.color = Color.white;

            // Pause menu title
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 200, 50), "PAUSADO", labelStyle);

            // Resume button
            if (GUI.Button(new Rect(Screen.width / 2 - 75, Screen.height / 2 - 50, 150, 40), "Resumir", buttonStyle))
            {
                ResumeGame();
            }

            // Restart button
            if (GUI.Button(new Rect(Screen.width / 2 - 75, Screen.height / 2, 150, 40), "Reiniciar", buttonStyle))
            {
                RestartGame();
            }

            // Quit button
            if (GUI.Button(new Rect(Screen.width / 2 - 75, Screen.height / 2 + 50, 150, 40), "Salir", buttonStyle))
            {
                QuitGame();
            }
        }
    }

    private void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
    }

    private void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
    }

    private void RestartGame()
	{
		// Reset time scale to normal
		Time.timeScale = 1;
		
		// Reload the current scene
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

    private void QuitGame()
    {
        // Reset time scale before quitting
        Time.timeScale = 1;
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
