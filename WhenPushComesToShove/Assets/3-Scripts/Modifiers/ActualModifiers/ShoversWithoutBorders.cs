using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoversWithoutBorders : BaseModifier
{
    public override void GameStart()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("ChalkLine");
     
        foreach(GameObject obj in objects)
        {
            obj.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
