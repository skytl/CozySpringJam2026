using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterTeleporterBox : MonoBehaviour
{
    public Collider2D[] tpboxColliders;
    public SpriteRenderer playerSprite;

    private int originalLayerOrder;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // first, verify the thing entering is a player, and not like, a wineglass, or something!
        if(collision.gameObject.tag == "Player")
        {
            foreach (Collider2D tpbox in tpboxColliders)
            {
                tpbox.isTrigger = true;
            }

            originalLayerOrder = playerSprite.sortingOrder;
            playerSprite.GetComponent<SpriteRenderer>().sortingOrder = -1;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        foreach (Collider2D tpbox in tpboxColliders)
        {
            tpbox.isTrigger = false;
        }
        
        playerSprite.GetComponent<SpriteRenderer>().sortingOrder = originalLayerOrder;
    }
}
