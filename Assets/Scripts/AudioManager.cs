using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource[] soundEffects;
    public AudioSource bgmSources, levelEndMusic;

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
        
    }

    public void PlaySFX(int soundToPlayIndex)
    {
        soundEffects[soundToPlayIndex].Stop();

        soundEffects[soundToPlayIndex].pitch = Random.Range(0.9f, 1.1f);

        soundEffects[soundToPlayIndex].Play();
    }

    public void PlayLvlVictory()
    {
        bgmSources.Stop();
        levelEndMusic.Play();
    }
}
