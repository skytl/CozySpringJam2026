using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassShatter : MonoBehaviour
{
    public GameObject[] wineGlassID;

    public SpriteRenderer sr;
    public Rigidbody2D rb;
    public Sprite brokenGlass;
    private bool willBreak = false;
    // private Vector2 glassPosition;
    bool isBroken = false;

    public BoxCollider2D glassCollider;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    private bool isGrounded;





    private void Update()
    {
        foreach(GameObject wineGlass in wineGlassID)
        {
            if (wineGlass != isBroken)
            {
                CheckGrounded();
                CheckFallDistance();
            }
        }
    }






    void CheckGrounded()
    {
        Debug.Log(rb.position.x.ToString());
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (willBreak && isGrounded)
        {
            /* note to self: whenever you get around to rewriting this script, have it set a 
            fall timer instead. If it doesn't fall very far, or, if it lands on something soft
            like a cushion, it should be intact after landing. */
            // glassPosition = rb.position;
            rb.SetRotation(0);
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            // rb.MovePosition(glassPosition);
            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
            rb.constraints = RigidbodyConstraints2D.FreezePositionY;
            glassCollider.isTrigger = true;
            sr.sprite = brokenGlass;
            isBroken = true;
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
