using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPoint : MonoBehaviour
{
    public MapPoint up, right, down, left;
    public bool isLevel, isLocked;
    public string levelToLoad, levelToCheck, levelName;

    public int collectedGemCount, totalGems;
    public float bestTime, targetTime;

    public GameObject gemBadge, timeBadge;

    // Start is called before the first frame update
    void Start()
    {
        if(isLevel && levelToLoad != null)
        {
            isLocked = true;

            if(PlayerPrefs.HasKey(levelToCheck + "_gems"))
            {
                collectedGemCount = PlayerPrefs.GetInt(levelToCheck + "_gems");
            }

            if (PlayerPrefs.HasKey(levelToCheck + "_time"))
            {
                bestTime = PlayerPrefs.GetFloat(levelToCheck + "_time");
            }

            if(collectedGemCount >= totalGems)
            {
                gemBadge.SetActive(true);
            }

            if(bestTime <= targetTime && bestTime != 0f)
            {
                timeBadge.SetActive(true);
            }

            if (levelToCheck != null)
            {
                if (PlayerPrefs.HasKey(levelToCheck + "_unlocked"))
                {
                    if(PlayerPrefs.GetInt(levelToCheck + "_unlocked") == 1)
                    {
                        isLocked = false;
                    }
                }
            }

            if(levelToLoad == levelToCheck)
            {
                isLocked = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
