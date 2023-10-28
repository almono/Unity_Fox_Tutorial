using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public Transform target;

    public float minHeight, maxHeight;

    public Transform farBackground, middleBackground, weatherEffect;

    private Vector2 lastPos;

    public bool stopFollow;

    public void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // make camera follow player/object -> with vertical limits
        // keep position Y between MIN and MAX heights so it does not go outside the bounds
        if(!stopFollow)
        {
            transform.position = new Vector3(target.position.x, Mathf.Clamp(target.position.y, minHeight, maxHeight), transform.position.z);

            // background parallax
            Vector2 amountToMove = new Vector2(transform.position.x - lastPos.x, transform.position.y - lastPos.y);

            farBackground.position += new Vector3(amountToMove.x, amountToMove.y, 0f); // always follow camera 1 to 1
            middleBackground.position += new Vector3(amountToMove.x, amountToMove.y, 0f) * 0.5f;
            weatherEffect.position += new Vector3(amountToMove.x, amountToMove.y, 0f); // weather effects should always follow camera

            lastPos = transform.position;
        }        
    }
}
