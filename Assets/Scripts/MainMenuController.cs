using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public string startScene, continueScene;

    public GameObject continueBtn;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey(startScene + "_unlocked"))
        {
            continueBtn.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene(startScene);
        PlayerPrefs.DeleteAll(); // Delete all saved player data
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(continueScene);
    }
}
