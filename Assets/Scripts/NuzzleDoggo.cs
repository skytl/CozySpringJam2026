using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NuzzleDoggo : MonoBehaviour
{
    public Collider2D dogCol;
    public SpriteRenderer srCringe;
    public SpriteRenderer srHappy;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Testing?");
            srCringe.enabled = false;
            srHappy.enabled = true;
        }
    }
}
