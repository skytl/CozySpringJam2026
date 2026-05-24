using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    /* Known Bug: every other jump, the player enters Idle anim, during the jump. Haven't been able to figure out the cause, yet */

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



    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    private bool isGrounded;


    [Header("Wall Check")]
    //public Transform wallCheck;
    //public Vector2 wallCheckVolume = new Vector2(.5f, .05f);
    // public LayerMask climbLayer;
    public Collider2D climbableVolume;
    private bool isClimbable = false;
    private bool isClimbing = false;
    


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
        /* Temporarily disabling ApplyVariableGravity, since it's messing with my Climb script*/
        ApplyVariableGravity();
        CheckGrounded();
        HandleMovement();
        HandleJump();
        //CheckInClimbVolume();
    }




    private void HandleMovement()
    {
        if (isClimbing == true)
        {
            float targetClimb = moveInput.y * speed;
            rb.linearVelocity = new Vector2(0, targetClimb);
        }
        else
        {
            float targetSpeed = moveInput.x * speed;
            rb.linearVelocity = new Vector2(targetSpeed, rb.linearVelocity.y);
        }
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


    void ApplyVariableGravity()
    {
        if (isClimbable == true && isClimbing == true) // climbing
        {
            rb.gravityScale = 0;
        }
        else if (isClimbing == false)
        {
            if (rb.linearVelocity.y < -0.1f) // falling
            {
                rb.gravityScale = fallGravity;
                Debug.Log("FAAAAALL");
            }
            else if (rb.linearVelocity.y > 0.1f) // rising
            {
                rb.gravityScale = jumpGravity;
                Debug.Log("up up and away");
            }
            else
            {
                rb.gravityScale = normalGravity;
                Debug.Log("normal");
            }
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
        Debug.Log($"Move Input: {moveInput.y}");

        if (isClimbable == true && moveInput.y == 1)
        {
            isClimbing = true;
            Debug.Log("I've started climbing");
        }

        if (isClimbing == true && moveInput.x >= .1f || moveInput.x < -.1f)
        {
            isClimbing = false;
            Debug.Log("I've started climbing");
        }
    }




    public void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            jumpPressed = true;
            jumpReleased = false;
            isClimbing = false;
        }
        else
        {
            jumpReleased = true;
        }
    }





    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == climbableVolume)
        {
            isClimbable = true;
        }
    }





    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == climbableVolume)
        {
            isClimbable=false;
            isClimbing=false;
            Debug.Log("Now leaving the facility");
        }
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
