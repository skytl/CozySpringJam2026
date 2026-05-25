using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassShatter : MonoBehaviour
{
    // public GameObject Wineglass_Broken;

    public SpriteRenderer sr;
    public Rigidbody2D rb;
    public Sprite brokenGlass;
    private bool willBreak = false;
    public Vector2 glassPosition;
    bool isBroken = false;

    //public BoxCollider2D glassCollider;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    private bool isGrounded;


    private void Awake()
    {
        
    }

    private void Update()
    {
        if (!isBroken)
        {
            CheckGrounded();
            CheckFallDistance();
        }
    }



    void CheckGrounded()
    {
        Debug.Log(rb.position.x.ToString());
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (willBreak && isGrounded)
        {
            glassPosition = rb.position;
            rb.SetRotation(0);
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            // glassCollider.isTrigger = true;
            // rb.MovePosition(glassPosition);
            // rb.constraints = RigidbodyConstraints2D.FreezePositionX;
            // rb.constraints = RigidbodyConstraints2D.FreezePositionY;
            sr.sprite = brokenGlass;
            isBroken = true;
            // rb.bodyType = RigidbodyType2D.Static;
        }
    }



    private void CheckFallDistance()
    {
        if (rb.linearVelocity.y < -.1f && !isGrounded)
        {
                willBreak = true;
        }
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
