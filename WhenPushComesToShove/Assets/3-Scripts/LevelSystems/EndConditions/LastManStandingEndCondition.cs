using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LastManStandingEndCondition : BaseEndCondition
{

    List<Health> playerHealth = new List<Health>();
    List<Health> playersToRemove = new List<Health>();

    [SerializeField] bool testForAllPlayersDead;
    int minPlayers;

    [HideInInspector]
    public Transform winner;

    protected  void Start()
    {
        if (testForAllPlayersDead)
            minPlayers = 0;
        else
            minPlayers = 1;
    }

    protected void OnEnable()
    {
        winner = null;
    }

    public override bool TestCondition()
    {
        Transform alivePlayer = null;
        foreach(Transform player in GameState.players)
        {
            PlayerHealth health = player.GetComponentInChildren<PlayerHealth>();
            if (!health.dead)
            {
                if(alivePlayer != null)
                {
                    return false;
                }
                alivePlayer = player;
            }

            winner = alivePlayer;
            return true;
        }

        Debug.LogError("NO PLAYERS?");
        return false;

      
    }

}
