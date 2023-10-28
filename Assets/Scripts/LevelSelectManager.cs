using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    public static LevelSelectManager instance;
    public LevelSelectPlayer player;

    private MapPoint[] allLevelPoints;

    public void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        allLevelPoints = FindObjectsOfType<MapPoint>();

        if(PlayerPrefs.HasKey("CurrentLevel"))
        {
            foreach(MapPoint levelPoint in allLevelPoints)
            {
                if(levelPoint.levelToLoad == PlayerPrefs.GetString("CurrentLevel"))
                {
                    player.transform.position = levelPoint.transform.position;
                    player.currentPoint = levelPoint;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevel()
    {
        StartCoroutine(LoadLevelCo());
    }

    public IEnumerator LoadLevelCo()
    {
        AudioManager.instance.PlaySFX(4);
        LevelSelectUIController.instance.FadeToBlack();
        yield return new WaitForSeconds((1f / LevelSelectUIController.instance.fadeSpeed) + 0.5f);

        SceneManager.LoadScene(player.currentPoint.levelToLoad);
    }
}
