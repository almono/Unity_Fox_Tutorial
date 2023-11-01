using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankBossController : MonoBehaviour
{
    public enum bossStates { shooting, hurt, moving };
    public bossStates currentState;

    [Header("General")]
    public Transform theBoss;
    public Animator bossAnimation;

    [Header("Movement")]
    public float moveSpeed = 12f;
    public Transform leftPoint, rightPoint;
    private bool movingRight = false;

    [Header("Attacks")]
    public GameObject bossBullet;
    public Transform firePoint; // from where it should shoot
    public float timeBetweenShots = 1.5f;
    private float shotCounter;

    [Header("Getting Hurt")]
    public float hurtTime = 4f; // how long to wait after getting hurt
    private float hurtCounter;

    // Start is called before the first frame update
    void Start()
    {
        currentState = bossStates.shooting;
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case bossStates.shooting:

                break;
            case bossStates.hurt: 
                
                if(hurtCounter > 0)
                {
                    hurtCounter -= Time.deltaTime;

                    if(hurtCounter <= 0)
                    {
                        currentState = bossStates.moving;
                    }
                }

                break;
            case bossStates.moving: 

                if(movingRight)
                {
                    theBoss.position += new Vector3(moveSpeed * Time.deltaTime, 0f, 0f);

                    if(theBoss.position.x >= rightPoint.position.x)
                    {
                        theBoss.localScale = Vector3.one;
                        movingRight = false;
                        EndMovement();
                    }
                } else
                {
                    theBoss.position -= new Vector3(moveSpeed * Time.deltaTime, 0f, 0f);

                    if (theBoss.position.x <= leftPoint.position.x)
                    {
                        // use scale for changing direction so the child components 
                        // stay assigned in proper places according to the parent
                        theBoss.localScale = new Vector3(-1f, 1f, 1f);
                        movingRight = true;
                        EndMovement();
                    }
                }
                
                break;
        }

        // Allow this code to be used only inside unity editor
#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.H))
        {
            TakeHit();
        }
#endif
    }

    public void TakeHit()
    {
        currentState = bossStates.hurt;
        hurtCounter = hurtTime;

        bossAnimation.SetTrigger("isHit");
    }

    private void EndMovement()
    {
        currentState = bossStates.shooting;
        shotCounter = timeBetweenShots;
        bossAnimation.SetTrigger("stopMoving");
    }
}
