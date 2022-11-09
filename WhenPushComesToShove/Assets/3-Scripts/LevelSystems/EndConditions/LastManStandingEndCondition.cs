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

    [SerializeField] private bool showWinnerText = true;
    private TextMeshProUGUI winnerText;

    protected  void Start()
    {
        if (testForAllPlayersDead)
            minPlayers = 0;
        else
            minPlayers = 1;
    }

    protected void OnEnable()
    {

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject obj in players)
        {
            playerHealth.Add(obj.GetComponentInChildren<Health>());
        }
    }

    protected override bool TestCondition()
    {
        
        foreach(Transform player in GameState.players)
        {
            if (!player.dead)
                playersToRemove.Add(player);
        }

      
    }

}
