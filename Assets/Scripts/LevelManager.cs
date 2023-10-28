using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public float respawnWaitTime;
    public int gemScore;
    public string levelToLoad;

    public float timeInLevel; // for counting how long it takes to beat level

    public void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        timeInLevel = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timeInLevel += Time.deltaTime;
    }

    public void RespawnPlayer()
    {
        StartCoroutine(RespawnCo());
    }

    public void KillPlayer()
    {
        PlayerController.instance.gameObject.SetActive(false);
        PlayerHealthController.instance.currentHealth = 0;
        UIController.instance.UpdateHealthDisplay();
    }

    // routine has to be started with a function
    private IEnumerator RespawnCo()
    {
        PlayerController.instance.SetDefaultValues();
        PlayerController.instance.gameObject.SetActive(false); // deactivate player on death
        AudioManager.instance.PlaySFX(8); // player death sfx
        PlayerHealthController.instance.currentHealth = 0;
        UIController.instance.UpdateHealthDisplay();

        // show black screen
        yield return new WaitForSeconds(respawnWaitTime - (1f / UIController.instance.fadeSpeed)); // wait for return before continuing the routine/function
        UIController.instance.FadeToBlack();

        // hide black screen
        yield return new WaitForSeconds((1f / UIController.instance.fadeSpeed) + 0.2f); // extra 0.2 to ensure it stays fully black a bit longer
        UIController.instance.FadeFromBlack();

        // Respawn player at checkpoint position with full health
        PlayerController.instance.gameObject.SetActive(true);
        PlayerController.instance.transform.position = CheckpointController.instance.spawnPoint;
        PlayerHealthController.instance.currentHealth = PlayerHealthController.instance.maxHealth;
        UIController.instance.UpdateHealthDisplay(); // update UI after setting health
    }

    public void EndLevel()
    {
        StartCoroutine(EndLevelCo());
    }

    public IEnumerator EndLevelCo()
    {
        AudioManager.instance.PlayLvlVictory();
        PlayerController.instance.stopInput = true;
        CameraController.instance.stopFollow = true; // stop camera follow
        PlayerController.instance.playerBody.velocity = Vector3.zero; // stop player
        UIController.instance.levelCompleteText.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        UIController.instance.FadeToBlack();

        yield return new WaitForSeconds((1 / UIController.instance.fadeSpeed) + 2.0f);

        // save data using PlayerPrefs
        string sceneName = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetInt(sceneName + "_unlocked", 1);
        PlayerPrefs.SetString("CurrentLevel", sceneName);
        
        // Save gems and time only if they are better than current "BEST" scores
        if(PlayerPrefs.HasKey(sceneName + "_gems"))
        {
            if(PlayerPrefs.GetInt(sceneName + "_gems") < gemScore)
            {
                PlayerPrefs.SetInt(sceneName + "_gems", gemScore);
            }
        } else
        {
            PlayerPrefs.SetInt(sceneName + "_gems", gemScore);
        }

        if (PlayerPrefs.HasKey(sceneName + "_time"))
        {
            if (PlayerPrefs.GetFloat(sceneName + "_time") > timeInLevel)
            {
                PlayerPrefs.SetFloat(sceneName + "_time", timeInLevel);
            }
        }
        else
        {
            PlayerPrefs.SetFloat(sceneName + "_time", timeInLevel);
        }     

        SceneManager.LoadScene(levelToLoad);
    }
}
