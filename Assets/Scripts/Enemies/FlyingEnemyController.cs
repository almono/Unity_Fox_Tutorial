using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyController : MonoBehaviour
{
    public Transform[] points;
    public float moveSpeed = 5f;
    private int currentPoint;
    public float distanceToAttackPlayer = 5f, chaseSpeed = 10f, waitAfterAttack = 2f;

    private Vector3 attackTarget;
    public float attackCounter = 0f;

    public SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < points.Length; i++)
        {
            points[i].parent = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(attackCounter > 0)
        {
            attackCounter -= Time.deltaTime;
        } else
        {
            if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) > distanceToAttackPlayer)
            {

                attackTarget = Vector3.zero; // reset attack target after player is out of reach

                // Follow usual route
                transform.position = Vector3.MoveTowards(transform.position, points[currentPoint].position, moveSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, points[currentPoint].position) < 0.05f)
                {
                    if (currentPoint == points.Length - 1)
                    {
                        currentPoint = 0;
                    }
                    else
                    {
                        currentPoint++;
                    }
                }

                if (transform.position.x < points[currentPoint].position.x)
                {
                    spriteRenderer.flipX = true;
                }
                else
                {
                    spriteRenderer.flipX = false;
                }
            }
            else
            {
                // Chase the player
                if (attackTarget == Vector3.zero)
                {
                    attackTarget = PlayerController.instance.transform.position;
                }

                transform.position = Vector3.MoveTowards(transform.position, attackTarget, chaseSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) <= 1f)
                {
                    attackCounter = waitAfterAttack;
                    attackTarget = Vector3.zero;
                }

                if (transform.position.x < PlayerController.instance.transform.position.x)
                {
                    spriteRenderer.flipX = true;
                }
                else
                {
                    spriteRenderer.flipX = false;
                }
            }
        }
    }
}
