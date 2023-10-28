using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;

    public GameObject pauseScreen;
    public string levelSelectScene, mainMenuScene;

    public bool isGamePaused = false;

    public void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
    }

    public void PauseUnpause()
    {
        if(isGamePaused)
        {
            isGamePaused = false;
            pauseScreen.SetActive(false);
            Time.timeScale = 1.0f;
        }
        else
        {
            isGamePaused = true;
            pauseScreen.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }

    public void LevelSelectOption()
    {
        PlayerPrefs.SetString("CurrentLevel", SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(levelSelectScene);
        Time.timeScale = 1.0f;
    }

    public void MainMenuOption()
    {
        SceneManager.LoadScene(mainMenuScene);
        Time.timeScale = 1.0f;
    }
}
