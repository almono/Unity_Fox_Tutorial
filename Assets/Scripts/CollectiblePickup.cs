using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblePickup : MonoBehaviour
{
    public bool isGem, isHeal;
    public int healValue;

    private bool isCollected;

    public GameObject pickupAnimation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !isCollected)
        {
            if(isGem)
            {
                LevelManager.instance.gemScore++;
                UIController.instance.UpdateGemScoreText();

                isCollected = true;
                Destroy(gameObject);

                // create animation
                Instantiate(pickupAnimation, transform.position, transform.rotation);
                AudioManager.instance.PlaySFX(6); // gem pickup sfx
            }

            if(isHeal)
            {
                if(PlayerHealthController.instance.currentHealth < PlayerHealthController.instance.maxHealth)
                {
                    PlayerHealthController.instance.HealPlayer(healValue);
                }

                isCollected = true;
                Destroy(gameObject);

                // create pickup animation
                Instantiate(pickupAnimation, transform.position, transform.rotation);
                AudioManager.instance.PlaySFX(7); // health pickup sfx
            }
        }
    }
}
