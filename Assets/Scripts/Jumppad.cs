using UnityEngine;

public class Jumppad : MonoBehaviour
{
    public float bounce = 20f;
    public float cushionBounce = 10f;
    public Rigidbody2D jumppadRb;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // TO DO: Figure out how to set this up so things like Fish, and Wineglasses can also be bounced off cushions
        // TO DO: Add a check to verify the cushion isGrounded, before anything can bounce off it. If it's not grounded, it should get knocked away, without bouncing things off it
        // TO DO: Add a check to make sure the thing bouncing off of it is either above or below (you shouldn't really bounce off the side of a cushion)
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 collisionVelocity = collision.relativeVelocity/2;
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * bounce, ForceMode2D.Impulse);
            jumppadRb.AddForce(Vector2.up * cushionBounce, ForceMode2D.Impulse);
            jumppadRb.AddForce(Vector2.one * collisionVelocity *-1, ForceMode2D.Impulse);
        }
    }

}
