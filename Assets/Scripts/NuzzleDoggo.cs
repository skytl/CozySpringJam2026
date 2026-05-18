using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NuzzleDoggo : MonoBehaviour
{
    public Collider2D dogCol;
    public SpriteRenderer srCringe;
    public SpriteRenderer srHappy;
    public CapsuleCollider2D capsule;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Testing?");
            srCringe.enabled = false;
            srHappy.enabled = true;
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // disable collision on doggo's head, so player can jump straight up if they want to
            capsule.enabled = false;
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // reenable collision on doggo's head
            capsule.enabled = true;
        }
    }
}
