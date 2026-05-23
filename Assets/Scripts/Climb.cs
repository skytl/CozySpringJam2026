using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climb : MonoBehaviour
{
    public float verticalMvmt;
    public float speed = 8f;
    private bool isClimbable;
    private bool isClimbing;
    private float originalGravityScale;

    // public CapsuleCollider2D playerCol;
    [SerializeField] private Rigidbody2D rb;


    private void Start()
    {
        originalGravityScale = rb.gravityScale;
    }


    private void Update()
    {
        verticalMvmt = Input.GetAxis("Vertical");

        if(isClimbable && Mathf.Abs(verticalMvmt) > 0f)
        {
            isClimbing = true;
        }
    }


    private void FixedUpdate()
    {
        if (isClimbing)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, verticalMvmt * speed);
        }
        else
        {
            rb.gravityScale = originalGravityScale;
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Climbable"))
        {
            isClimbable = true;
        }

    }

    /*
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(gameObject.tag == "Player")
        {
            // Physics.gravity = new Vector3(0, 0, 0);
        }
    }
    */


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Climbable"))
        {
            isClimbable = false;
            isClimbing = false;
        }
    }
}
