using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectUIController : MonoBehaviour
{
    public static LevelSelectUIController instance;
    public Image FadeScreen;
    public float fadeSpeed;
    private bool shouldFadeToBlack, shouldFadeFromBlack; // is it fade in or fade out effect

    public GameObject levelInfoPanel;
    public Text levelName, gemCollectedText, gemInLevelText, bestTimeText, timeTargetText;

    public void Awake()
    {
        instance = this;
        instance.gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        FadeFromBlack();
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldFadeToBlack)
        {
            FadeScreen.color = new Color(FadeScreen.color.r, FadeScreen.color.g, FadeScreen.color.b, Mathf.MoveTowards(FadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime)); // if it was only deltaTime then it would take 1s to reach that value, with fadeSpeed set to 3 it will take 1/3s

            if (FadeScreen.color.a >= 1f)
            {
                shouldFadeToBlack = false;
            }
        }

        if (shouldFadeFromBlack)
        {
            FadeScreen.color = new Color(FadeScreen.color.r, FadeScreen.color.g, FadeScreen.color.b, Mathf.MoveTowards(FadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime)); // if it was only deltaTime then it would take 1s to reach that value, with fadeSpeed set to 3 it will take 1/3s

            if (FadeScreen.color.a <= 0f)
            {
                shouldFadeFromBlack = false;
            }
        }
    }

    public void FadeToBlack()
    {
        shouldFadeToBlack = true;
        shouldFadeFromBlack = false;
    }

    public void FadeFromBlack()
    {
        shouldFadeToBlack = false;
        shouldFadeFromBlack = true;
    }

    public void ShowLevelInfo(MapPoint mapPoint)
    {
        levelName.text = mapPoint.levelName;

        gemCollectedText.text = "FOUND: " + mapPoint.collectedGemCount.ToString();
        gemInLevelText.text = "IN LEVEL: " + mapPoint.totalGems.ToString();
        timeTargetText.text = "TARGET: " + mapPoint.targetTime + "s";

        if(mapPoint.bestTime != 0f)
        {
            bestTimeText.text = "BEST: " + mapPoint.bestTime.ToString("F1") + "s";
        } else
        {
            bestTimeText.text = "BEST: ---";
        }

        if (mapPoint.isLocked)
        {
            levelName.text += " ( locked )";
        }

        if(mapPoint.isLevel)
        {
            levelInfoPanel.SetActive(true);
        } else
        {
            levelInfoPanel.SetActive(false);
        }
    }

    public void HideLevelInfo()
    {
        levelInfoPanel.SetActive(false);
    }
}
