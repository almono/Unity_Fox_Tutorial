using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPadController : MonoBehaviour
{
    public Animator anim;
    public float bounceForce = 20f;

    // Start is called before the first frame update
    void Start()
    {
        this.anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            PlayerController.instance.playerBody.velocity = new Vector2(PlayerController.instance.playerBody.velocity.x, bounceForce);
            anim.SetTrigger("BounceTrigger");
        }
    }
}
