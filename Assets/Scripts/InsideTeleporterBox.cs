using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideTeleporterBox : MonoBehaviour
{
    public Collider2D col;
    public Collider2D[] tpboundaryColliders;
    public SpriteRenderer playerSprite;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            foreach (Collider2D tpboundary in tpboundaryColliders)
            {
                tpboundary.enabled = true;
            }
        }
        
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        playerSprite.GetComponent<SpriteRenderer>().color = Color.clear;
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        playerSprite.GetComponent<SpriteRenderer>().color = Color.white;
        foreach (Collider2D tpboundary in tpboundaryColliders)
        {
            tpboundary.enabled = false;
        }
    }
}
