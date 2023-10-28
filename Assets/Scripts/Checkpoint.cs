using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public SpriteRenderer checkpointSprite;
    public Sprite checkpointOn, checkpointOff;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            CheckpointController.instance.DeactivateCheckpoints(); // deactive all and then activate the one Player activated
            checkpointSprite.sprite = checkpointOn;
            CheckpointController.instance.SetSpawn(transform.position);
        }
    }

    public void ResetCheckpoint()
    {
        checkpointSprite.sprite = checkpointOff;
    }
}
