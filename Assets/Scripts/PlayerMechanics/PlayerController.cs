using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    // Main mechanics
    public Rigidbody2D playerBody;
    public bool collisionEnabled = true;

    // Move and Jump
    public float moveSpeed;
    public float jumpForce;
    public float doubleJumpForce;
    private bool canDoubleJump;

    // Ground detection
    private bool isGrounded;
    public Transform groundDetector;
    public LayerMask whatIsGround;

    // Player Animations and Sprites
    private Animator playerAnim;
    private SpriteRenderer playerSprite;
    private bool facingLeft = false;

    // Knockback
    public float knockbackLength, knockbackForce;
    private float knockbackCounter;

    // Dash
    public bool canDash = true;
    private bool isDashing = false;
    public float dashSpeed = 6f;
    public float dashLength = 0.45f;
    public float dashCooldown = 2.5f; // Cooldown period between dashes.

    // Barier
    public GameObject playerBarrier;
    public float barrierMaxUptime = 1f;
    public float barrierCooldown = 3f;
    protected float barrierMaxSize = 0.9f;
    private bool canDeployBarrier = true;

    // Other

    public float bounceForce;
    public bool stopInput;

    public void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.instance.isGamePaused && !stopInput)
        {
            // if player control is allowed ( is in knockback state )
            if (knockbackCounter <= 0 && !isDashing)
            {
                // GetAxis -> speed to max over time ( momentum )
                // GetAxisRaw -> speed either 0 or 1
                playerBody.velocity = new Vector2(moveSpeed * Input.GetAxisRaw("Horizontal"), playerBody.velocity.y);
                isGrounded = Physics2D.OverlapCircle(groundDetector.position, .2f, whatIsGround);

                if (isGrounded)
                {
                    canDoubleJump = true;
                }

                if (Input.GetButtonDown("Jump"))
                {
                    if (isGrounded)
                    {
                        playerBody.velocity = new Vector2(playerBody.velocity.x, jumpForce);
                        AudioManager.instance.PlaySFX(10); // player jump sfx
                    }
                    else if (canDoubleJump)
                    {
                        playerBody.velocity = new Vector2(playerBody.velocity.x, doubleJumpForce);
                        AudioManager.instance.PlaySFX(10); // player jump sfx
                        canDoubleJump = false;
                    }

                }

                if (playerBody.velocity.x < 0)
                {
                    playerSprite.flipX = true;
                    facingLeft = true;
                }
                else if (playerBody.velocity.x > 0)
                {
                    playerSprite.flipX = false;
                    facingLeft = false;
                }
            }
            else if(!isDashing)
            {
                knockbackCounter -= Time.deltaTime;

                if (!playerSprite.flipX)
                {
                    playerBody.velocity = new Vector2(-knockbackForce, playerBody.velocity.y);
                }
                else
                {
                    playerBody.velocity = new Vector2(knockbackForce, playerBody.velocity.y);
                }
            }

            if(canDash && !isDashing && Input.GetKey(KeyCode.LeftShift)) 
            {
                StartCoroutine("Dash");
            }

            if(canDeployBarrier && Input.GetKey(KeyCode.LeftControl))
            {
                StartCoroutine("CreateBarrier");
            }
        }   

        playerAnim.SetBool("isGrounded", isGrounded);
        playerAnim.SetFloat("moveSpeed", Mathf.Abs(playerBody.velocity.x));
        
    }

    public void Knockback()
    {
        knockbackCounter = knockbackLength;
        playerBody.velocity = new Vector2(0f, knockbackForce);

        playerAnim.SetTrigger("isHurt");
    }

    public void Bounce()
    {
        playerBody.velocity = new Vector2(playerAnim.velocity.x, bounceForce);
        AudioManager.instance.PlaySFX(10); // player jump sfx
    }

    // Function to set values back to default if needed ( on death etc. )
    public void SetDefaultValues()
    {
        collisionEnabled = true;
        canDeployBarrier = true;
        canDash = true;
        playerBarrier.SetActive(false);
        isDashing = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Platform")
        {
            // make platform parent of player
            transform.parent = other.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Platform")
        {
            // remove platform parent of player
            transform.parent = null;
        }
    }

    public IEnumerator CreateBarrier()
    {
        playerBarrier.SetActive(true);
        ShieldCooldownBar.instance.StartCooldown(barrierCooldown + barrierMaxUptime);
        canDeployBarrier = false;
        //playerBarrier.transform.localScale = new Vector3 * Mathf.PingPong(0.8f, 1f);

        // scale barrier up
        while (playerBarrier.gameObject.transform.localScale.x < barrierMaxSize)
        {
            playerBarrier.gameObject.transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime * 6f;
            yield return null;
        }

        yield return new WaitForSeconds(barrierMaxUptime);

        // scale barrier down
        while (playerBarrier.gameObject.transform.localScale.x > 0.1f)
        {
            playerBarrier.gameObject.transform.localScale -= new Vector3(1, 1, 1) * Time.deltaTime * 6f;
            yield return null;
        }

        yield return new WaitForSeconds(barrierCooldown);
        canDeployBarrier = true;
    }

    public IEnumerator Dash()
    {
        playerAnim.SetBool("isDashing", true);

        collisionEnabled = false;
        isDashing = true;
        canDash = false;
        Vector2 dashDirection = new Vector2(0f, 0f);

        if(facingLeft) 
        {
            dashDirection = new Vector2(-dashSpeed, 0);
        } else
        {
            dashDirection = new Vector2(dashSpeed, 0);
        }

        float distanceTraveled = 0f;

        while (distanceTraveled < dashLength)
        {
            // Set the player's velocity to move them in the dash direction at the specified speed.
            playerBody.velocity = dashDirection * dashSpeed;

            distanceTraveled += dashSpeed * Time.deltaTime;
            yield return null; // needed to break the loop and avoid infinite
        }

        isDashing = false;
        collisionEnabled = true;
        playerAnim.SetBool("isDashing", false);

        playerBody.velocity = Vector2.zero; // Stop the player when the dash is done.
        DashCooldownBar.instance.StartCooldown(dashCooldown);
        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }
}
