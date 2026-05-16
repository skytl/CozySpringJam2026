using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterTeleporterBox : MonoBehaviour
{
    public Collider2D colID;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // first, verify the thing entering is a player, and not like, a wineglass, or something!
        // then, figure out how to turn off collision for the targeted Box Collider 2D (or some other way to effectively let the player "enter" the box);
        // pretty sure we did stuff like that in the rpg tutorial @@
        Debug.Log("BOX is currently closed for repairs. Please check back in a later build. Thank you!");
    }


}
