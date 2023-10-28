using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            // Deal damage only if the player collision is enabled
            if(PlayerController.instance.collisionEnabled)
            {
                //FindObjectOfType<PlayerHealthController>().DealDamage(); // not efficient, not performance friendly, lack of control
                PlayerHealthController.instance.DealDamage(); // because its a singleton we can call it everywhere
                AudioManager.instance.PlaySFX(9); // player hurt sfx
            }
        }
    }
}
