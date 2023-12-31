using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankBossMine : MonoBehaviour
{
    public GameObject explosion;
    public float lifeTime = 3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Destroy(gameObject);
            Instantiate(explosion, transform.position, transform.rotation);
            PlayerHealthController.instance.DealDamage();
            AudioManager.instance.PlaySFX(3);
        }
    }

    public void Explode()
    {
        Destroy(gameObject);
        AudioManager.instance.PlaySFX(3);
        Instantiate(explosion, transform.position, transform.rotation);
    }
}
