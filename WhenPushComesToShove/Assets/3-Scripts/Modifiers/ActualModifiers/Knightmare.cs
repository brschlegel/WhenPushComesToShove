using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knightmare : BaseModifier
{
    Transform[] players;
    
    public override void Init()
    {
        players = GameState.players.ToArray();
        Debug.Log(players.Length + " length");

        foreach(Transform player in players)
        {
            player.GetChild(14).gameObject.SetActive(true);
        }
    }

    public override void CleanUp()
    {
        foreach (Transform player in players)
        {
            player.GetChild(14).gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        CleanUp();
    }
}
