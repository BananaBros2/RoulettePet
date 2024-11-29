using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseObject;
    public GameObject exitButton;
    public bool gamePaused;
    private float normalTimeScale = 1;

    // Start is called before the first frame update

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;

        pauseObject.SetActive(false);

#if !UNITY_EDITOR && UNITY_WEBGL
        Destroy(exitButton);
#endif
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            FlipPause();
        }
    }

    public void TimePauseAdjustment(bool paused)
    {
        if (paused)
        {
            gamePaused = true;

            normalTimeScale = Time.timeScale;
            Time.timeScale = 0.0001f;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            gamePaused = false;

            Time.timeScale = normalTimeScale;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void FlipPause()
    {
        pauseObject.SetActive(!pauseObject.activeInHierarchy);
        TimePauseAdjustment(pauseObject.activeInHierarchy);

    }

    public void ExitToMenu()
    {
        Time.timeScale = normalTimeScale;
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
        Destroy(pauseObject);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
