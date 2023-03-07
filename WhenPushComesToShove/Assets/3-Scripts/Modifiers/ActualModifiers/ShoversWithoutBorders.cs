using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoversWithoutBorders : BaseModifier
{
    GameObject[] objects;
    public override void GameInit()
    {
        objects = GameObject.FindGameObjectsWithTag("ChalkLine");
    }

    public override void GameStart()
    {
        
        Debug.Log(objects.Length);

        foreach(GameObject obj in objects)
        {
            obj.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
