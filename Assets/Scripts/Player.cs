using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    /* #1, set a boolean, that tracks whether JungleMode is false or true (default is false);
     * when playerInput == shift key, set JungleMode to true;
     * 
     */


    /* Known Bug: every other jump, the player enters Idle anim, during the jump. Haven't been able to figure out the cause, yet */

    // EXPERIMENTING!!
    //public ItemSO itemSO;
    public SpriteRenderer sr;
    // public int sortingOrder;

    [Header("Components")]
    public Rigidbody2D rb;
    public PlayerInput playerInput;
    public Animator anim;


    [Header("Movement Variables")]
    public float speed;
    public float jumpForce;
    public float jumpCutMultiplier = .5f;
    public float normalGravity;
    public float fallGravity;
    public float jumpGravity;


    public int facingDirection = 1;

    //Inputs are tracked here
    private Vector2 moveInput;
    private bool jumpPressed;
    private bool jumpReleased;
    private bool shiftPressed;
    private bool shiftReleased;


    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    private bool isGrounded;


    [Header("Wall Check")]
    public Transform wallCheck;
    public float wallCheckRadius; 
    public Vector2 wallCheckVol = new Vector2(.5f, .05f);
    public LayerMask wallLayer;
    private bool isClimbing;



    private void Start()
    {
        rb.gravityScale = normalGravity;
    }




    private void Update()
    {
        Flip();
        HandleAnimations();
    }




    void FixedUpdate()
    {
        ApplyVariableGravity();
        CheckGrounded();
        HandleMovement();
        HandleJump();
        // HandleShiftReality();
    }




    private void HandleMovement()
    {
        float targetSpeed = moveInput.x * speed;
        rb.linearVelocity = new Vector2(targetSpeed, rb.linearVelocity.y);
    }




    private void HandleJump()
    {
        if (jumpPressed && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpPressed = false;
            jumpReleased = false;
        }
        if (jumpReleased)
        {
            if (rb.linearVelocity.y > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
            }
            jumpReleased = false;
        }
    }


    /*
    // Note to self: you can remove this now, if you want. I've scrapped this concept
    private void HandleShiftReality()
    {
        if (shiftPressed)
        {
            sr.enabled = false;

            Debug.Log("Yo, this is trippy!");
            shiftPressed = false;
            shiftReleased = false;
        }
        if (shiftReleased)
        {
            Debug.Log("Meh, I've seen this before.");
            shiftReleased = false;
        }
    }
    */




    void ApplyVariableGravity()
    {
        if(rb.linearVelocity.y < -0.1f) // falling
        {
            rb.gravityScale = fallGravity;
        }
        else if(rb.linearVelocity.y > 0.1f) // rising
        {
            rb.gravityScale = jumpGravity;
        }
        else
        {
            rb.gravityScale = normalGravity;
        }
    }



    void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }



    void HandleAnimations()
    {
        anim.SetBool("isJumping", rb.linearVelocity.y > .1f);
        anim.SetBool("isGrounded", isGrounded);

        anim.SetFloat("yVelocity", rb.linearVelocity.y);

        anim.SetBool("isIdle", Mathf.Abs(moveInput.x) < .1f && isGrounded);
        anim.SetBool("isWalking", Mathf.Abs(moveInput.x) > .1f && isGrounded);
    }



    void Flip()
    {
        if(moveInput.x > 0.1f)
        {
            facingDirection = 1;
        }
        else if(moveInput.x < -0.1f)
        {
            facingDirection = -1;
        }

        transform.localScale = new Vector3(facingDirection, 1, 1);
    }






    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }



    public void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            jumpPressed = true;
            jumpReleased = false;
        }
        else
        {
            jumpReleased = true;

        }

    }


    /*
    public void OnShiftReality(InputValue value)
    {
        if (value.isPressed)
        {
            shiftPressed = true;
            shiftReleased = false;
        }
        else
        {
            shiftReleased = true;

        }

    }
    */



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawWireCube(wallCheck.position, wallCheckVol);
    }
}
