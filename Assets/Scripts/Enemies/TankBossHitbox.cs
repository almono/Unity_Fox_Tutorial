using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankBossHitbox : MonoBehaviour
{
    public TankBossController bossCont;

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
        // player has to be ABOVE the hitbox for it to be triggered
        if(other.tag == "Player" && PlayerController.instance.transform.position.y > transform.position.y)
        {
            bossCont.TakeHit();

            PlayerController.instance.Bounce();

            // deactivate hitbox so it cant be spammed
            gameObject.SetActive(false);
        }
    }
}
