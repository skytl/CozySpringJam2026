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

    private float coyoteTime = .2f;
    private float coyoteTimeCounter;

    private float jumpBuffer = .2f;
    private float jumpBufferCounter;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    private bool isGrounded;


    [Header("Climb Check")]
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

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

    }




    void FixedUpdate()
    {
        ApplyVariableGravity();
        CheckGrounded();
        HandleMovement();
        HandleJump();
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
        if (jumpPressed)
        {
            jumpBufferCounter = jumpBuffer;

            if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jumpPressed = false;
                jumpReleased = false;
                jumpBufferCounter = 0f;
            }
        }
        if (jumpReleased)
        {
            if (rb.linearVelocity.y > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
            }

            jumpReleased = false;
            coyoteTimeCounter = 0f;
            jumpBufferCounter -= Time.deltaTime;
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
            }
            else if (rb.linearVelocity.y > 0.1f) // rising
            {
                rb.gravityScale = jumpGravity;
            }
            else
            {
                rb.gravityScale = normalGravity;
            }
        }
    }



    void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }




    void HandleAnimations()
    {
        anim.SetBool("isJumping", rb.linearVelocity.y > .1f && !isClimbing);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isClimbing", rb.linearVelocity.y > .1f || rb.linearVelocity.y < -.1f && isClimbing);
        anim.SetBool("isHanging", rb.linearVelocity.y == 0f && isClimbing);

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
        // Debug.Log($"Move Input: {moveInput.y}");

        if (isClimbable == true && moveInput.y == 1)
        {
            isClimbing = true;
        }

        if (isClimbing == true && moveInput.x >= .1f || moveInput.x < -.1f)
        {
            isClimbing = false;
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
        }
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
