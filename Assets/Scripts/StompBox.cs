using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompBox : MonoBehaviour
{
    public GameObject deathEffect;
    public GameObject collectible;
    [Range(0, 100)] public float dropChance; // 0-100% chance

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
        if(other.CompareTag("Enemy"))
        {
            AudioManager.instance.PlaySFX(3); // enemy explode sfx
            other.transform.parent.gameObject.SetActive(false);
            Instantiate(deathEffect, other.transform.position, other.transform.rotation);            

            PlayerController.instance.Bounce();

            // drop collectible part
            float dropSelect = Random.Range(0, 100f);

            if(dropSelect < dropChance)
            {
                Instantiate(collectible, other.transform.position, other.transform.rotation);
            }
        }
    }
}
