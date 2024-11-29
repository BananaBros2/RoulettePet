using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public GameObject exitButton;

    // Start is called before the first frame update
    void Start()
    {

#if !UNITY_EDITOR && UNITY_WEBGL
        Destroy(exitButton);
#endif

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadGame()
    {
        SceneManager.LoadScene("GameMap", LoadSceneMode.Single); // Load the first scene
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
