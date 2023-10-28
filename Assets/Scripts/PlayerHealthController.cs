using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance; // singleton

    public int currentHealth, maxHealth;

    public float iFramesLength;
    private float iFramesCounter; // countdown to 0, before 0 we cant take damage
    private SpriteRenderer spriteRender;
    public GameObject deathEffect;

    // happens right before Start() function
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        spriteRender = GetComponent<SpriteRenderer>(); // to access values from SpriteRenderer component on player
    }

    // Update is called once per frame
    void Update()
    {
        // iframes counter
        if(iFramesCounter > 0)
        {
            iFramesCounter -= Time.deltaTime; // deltaTime relies on frames, if 60 fps then it becomes 1/60th of a second, on 30 fps 1/30th etc. => 1 second to take 1 value away

            // will take place only ONCE
            if(iFramesCounter <= 0)
            {
                spriteRender.color = new Color(spriteRender.color.r, spriteRender.color.g, spriteRender.color.b, 1.0f); // make player fully visible
            }
        }
    }

    public void DealDamage() 
    {
        if(iFramesCounter <= 0)
        {
            currentHealth -= 1;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                Instantiate(deathEffect, transform.position, transform.rotation);
                //gameObject.SetActive(false); // make player (object this script is attached to) disappear/die
                LevelManager.instance.RespawnPlayer(); 
            }
            else
            {
                iFramesCounter = iFramesLength;
                spriteRender.color = new Color(spriteRender.color.r, spriteRender.color.g, spriteRender.color.b, 0.5f); // make player half transparent
                PlayerController.instance.Knockback();
                AudioManager.instance.PlaySFX(9); // player hurt sfx
            }

            UIController.instance.UpdateHealthDisplay();
        }
    }

    public void HealPlayer(int HealAmount)
    {
        currentHealth += HealAmount;

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UIController.instance.UpdateHealthDisplay();
    }
}
