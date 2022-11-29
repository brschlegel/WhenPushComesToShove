using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LastTeamStanding : BaseEndCondition
{
    List<PlayerConfiguration> players = new List<PlayerConfiguration>();
    List<PlayerConfiguration> playersToRemove = new List<PlayerConfiguration>();
    [HideInInspector]
    public int winningTeamNum;

    public override void Init()
    {
        players = PlayerTeamFormations.instance.GetPlayerTeams();
        winningTeamNum = -1;
        playersToRemove = new List<PlayerConfiguration>();
    }

    public override bool TestCondition()
    {
        foreach(PlayerConfiguration player in players)
        {
            if(player.IsDead)
                playersToRemove.Add(player);
        }

        foreach(PlayerConfiguration player in playersToRemove)
        {
            Debug.Log("Removing player");
            players.Remove(player);
        }

        //Grab the first player configration as a base to compare the rest
        PlayerConfiguration basePlayer = players[0];

        for(int i = 1; i < players.Count; i++)
        {
            //Exit the code if there's still players of different teams
            if (players[i].TeamIndex != basePlayer.TeamIndex)
            {
                return false;
            }

            Debug.Log(players[i].TeamIndex);
               
        }
        winningTeamNum = players[0].TeamIndex;
        return true;
        //base.TestCondition();
    }
}
