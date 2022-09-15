using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastManStandingEndCondition : BaseEndCondition
{

    List<Health> playerHealth = new List<Health>();
    List<Health> playersToRemove = new List<Health>();

    [SerializeField] bool testForAllPlayersDead;
    int minPlayers;

    protected override void Start()
    {
        if (testForAllPlayersDead)
            minPlayers = 0;
        else
            minPlayers = 1;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach(GameObject obj in players)
        {
            playerHealth.Add(obj.GetComponent<Health>());
        }

        base.Start();
    }

    protected override void TestCondition()
    {
        foreach(Health player in playerHealth)
        {
            if (player.dead)
                playersToRemove.Add(player);
        }

        foreach(Health player in playersToRemove)
        {
            playerHealth.Remove(player);
        }

        if(playerHealth.Count <= minPlayers)
            base.TestCondition();
    }

    protected override IEnumerator TransitionRooms()
    {
        yield return delay;
        LevelManager.onEndGame.Invoke();
    }
}
