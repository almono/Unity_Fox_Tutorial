using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmasherEnemyController : MonoBehaviour
{
    public float fallSpeed = 5f, waitAfterSlam = 1f, slamCooldown = 2f;
    public Transform slammerBody, slammerTarget;

    private bool isSlamming = false, resetting = false;
    private float waitCounter;
    private Vector3 startPoint;

    // Start is called before the first frame update
    void Start()
    {
        startPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // If not falling and not going back up
        if(!isSlamming && !resetting)
        {
            if (Vector2.Distance(slammerTarget.position, PlayerController.instance.transform.position) < 2.5f)
            {
                isSlamming = true;
                waitCounter = slamCooldown;
            }
        }

        if(isSlamming)
        {
            slammerBody.position = Vector3.MoveTowards(slammerBody.position, slammerTarget.position, fallSpeed * Time.deltaTime);

            if(slammerBody.position.y <= slammerTarget.position.y)
            {
                // wait before going back up
                waitCounter -= Time.deltaTime;
                if(waitCounter <= 0)
                {
                    isSlamming = false;
                    resetting = true;
                }
            }
        }

        if(resetting)
        {
            slammerBody.position = Vector3.MoveTowards(slammerBody.position, startPoint, fallSpeed * Time.deltaTime);

            if (slammerBody.position == startPoint)
            {
                resetting = false;
            }
        }
    }
}
