using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankBossController : MonoBehaviour
{
    public enum bossStates { shooting, hurt, moving, ended };
    public bossStates currentState;

    [Header("General")]
    public Transform theBoss;
    public Animator bossAnimation;

    [Header("Movement")]
    public float moveSpeed = 12f;
    public Transform leftPoint, rightPoint, minePoint;
    private bool movingRight = false;
    public GameObject bossMine;
    public float timeBetweenMines = 1f;
    private float mineCounter = 0f;

    [Header("Attacks")]
    public GameObject bossBullet;
    public Transform firePoint; // from where it should shoot
    public float timeBetweenShots = 1.5f;
    private float shotCounter;

    [Header("Getting Hurt")]
    public float hurtTime = 4f; // how long to wait after getting hurt
    private float hurtCounter;
    public GameObject bossHitbox;

    [Header("Health")]
    public int health = 1;
    public GameObject explosion, winPlatforms;
    private bool isDefeated = false;
    public float shotSpeedUp = 1.2f, mineSpeedUp = 1.2f; // multiply speed by these values so each time +20%

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

                shotCounter -= Time.deltaTime;

                if(shotCounter <= 0)
                {
                    shotCounter = timeBetweenShots;

                    // create new bullet instance
                    var newBullet = Instantiate(bossBullet, firePoint.position, firePoint.rotation);
                    newBullet.transform.localScale = theBoss.localScale;
                }

                break;
            case bossStates.hurt: 
                
                if(hurtCounter > 0)
                {
                    hurtCounter -= Time.deltaTime;

                    if(hurtCounter <= 0)
                    {
                        currentState = bossStates.moving;
                        mineCounter = 0f;

                        if (isDefeated)
                        {
                            theBoss.gameObject.SetActive(false);
                            Instantiate(explosion, theBoss.position, theBoss.rotation);

                            currentState = bossStates.ended; // dont need case because nothing is going to happen in it
                            winPlatforms.SetActive(true);
                            AudioManager.instance.StopBossMusic();
                        }
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

                mineCounter -= Time.deltaTime;

                if (mineCounter <= 0f)
                {
                    mineCounter = timeBetweenMines;
                    Instantiate(bossMine, minePoint.position, minePoint.rotation);
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
        AudioManager.instance.PlaySFX(0);

        bossAnimation.SetTrigger("isHit");

        
        // In case we want to destroy all mines after each movement
          
        TankBossMine[] mines = FindObjectsOfType<TankBossMine>();
        if(mines.Length > 0)
        {
            foreach(TankBossMine mine in mines)
            {
                mine.Explode();
            }
        }

        health--;

        if(health <= 0)
        {
            isDefeated = true;
        } else
        {
            // make fight harder with each hit
            timeBetweenShots /= shotSpeedUp;
            timeBetweenMines /= mineSpeedUp;
        }
    }

    private void EndMovement()
    {
        currentState = bossStates.shooting;
        shotCounter = timeBetweenShots;
        bossAnimation.SetTrigger("stopMoving");
        bossHitbox.SetActive(true);
    }
}
