using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NuzzleDoggo : MonoBehaviour
{
    public Collider2D dogCol;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // verify collision object has the Player tag;
        // set the doggo's anim to be the happy version instead!;
        Debug.Log("TO DO: change doggo to be happy, after being nuzzled by cat");
    }
}
