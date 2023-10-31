using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverController : MonoBehaviour
{
    public GameObject objectToSwitch;
    private SpriteRenderer leverSprite;
    public Sprite downSprite;

    public bool isSwitched = false;
    public bool deactivateOnSwitch = true;

    // Start is called before the first frame update
    void Start()
    {
        leverSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && !isSwitched)
        {
            if(deactivateOnSwitch)
            {
                objectToSwitch.SetActive(false);
            } else
            {
                objectToSwitch.SetActive(true);
            }

            
            leverSprite.sprite = downSprite;
            isSwitched = true;
        }
    }
}
