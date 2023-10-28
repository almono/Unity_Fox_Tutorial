using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed;
    public float moveTime, waitTime; // how long to move for and how long to wait for
    private float moveCounter, waitCounter; // timers for above

    public Transform leftPoint, rightPoint;

    private bool movingRight;

    private Rigidbody2D enemyBody;
    private Animator enemyAnimation;

    public SpriteRenderer enemySprite;

    // Start is called before the first frame update
    void Start()
    {
        enemyBody = GetComponent<Rigidbody2D>();
        enemyAnimation = GetComponent<Animator>();

        // we remove points from children, this is to not clutter hierarchy with tons of empty objects ( we can collapse parent with them as children )
        leftPoint.parent = null; 
        rightPoint.parent = null;

        moveCounter = moveTime;
        waitCounter = waitTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveCounter > 0)
        {
            moveCounter -= Time.deltaTime;

            if (!movingRight)
            {
                // moving left
                enemyBody.velocity = new Vector2(-moveSpeed, enemyBody.velocity.y);

                if (transform.position.x < leftPoint.position.x)
                {
                    movingRight = true;
                    enemySprite.flipX = true;
                }
            }
            else
            {
                // moving right
                enemyBody.velocity = new Vector2(moveSpeed, enemyBody.velocity.y);

                if (transform.position.x > rightPoint.position.x)
                {
                    movingRight = false;
                    enemySprite.flipX = false;

                }
            }

            // nake so enemy will wait for X time after move counter reaches 0
            if(moveCounter <= 0)
            {
                waitCounter = Random.Range(waitTime * 0.75f, waitTime * 1.25f);
            }

            enemyAnimation.SetBool("isMoving", true);
        } else if(waitCounter > 0) 
        {
            waitCounter -= Time.deltaTime;
            enemyBody.velocity = new Vector2(0, enemyBody.velocity.y);

            // make enemy move again for X time after wait counter reaches 0
            if(waitCounter <= 0)
            {
                moveCounter = Random.Range(moveTime * 0.75f, moveTime * 1.25f);
            }

            enemyAnimation.SetBool("isMoving", false);
        }
        
    }
}
