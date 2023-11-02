using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankBossBulletController : MonoBehaviour
{
    public float bulletSpeed = 7f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // get boss scale ( -1 and 1 for left/right rotation )
        // based on boss scale move left or right
        transform.position += new Vector3(-bulletSpeed * transform.localScale.x * Time.deltaTime, 0f, 0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            PlayerHealthController.instance.DealDamage();
        }

        Destroy(gameObject);
    }
}
