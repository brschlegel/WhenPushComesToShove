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

    protected override void Start()
    {
        if (testForAllPlayersDead)
            minPlayers = 0;
        else
            minPlayers = 1;

        base.Start();
    }

    protected void OnEnable()
    {
        if (winnerText == null)
        {
            winnerText = UIManager.instance.victoryText;
            winnerText.gameObject.SetActive(false);
        }

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject obj in players)
        {
            playerHealth.Add(obj.GetComponentInChildren<Health>());
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        winnerText.gameObject.SetActive(false);
        playerHealth.Clear();
        playersToRemove.Clear();
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
        {
            //If winner text exists, display winner
            if (minPlayers == 1 && showWinnerText)
            {
                DisplayWinner(playerHealth[0].transform.parent.GetComponentInChildren<PlayerInputHandler>().playerConfig.PlayerColorName);
            }

            base.TestCondition();
        }
    }

    protected override IEnumerator TransitionRooms()
    {
        yield return delay;
        LevelManager.onEndGame.Invoke();
    }

    private void DisplayWinner(string playerName)
    {
        winnerText.gameObject.SetActive(true);
        winnerText.text = playerName + " Player Won!";
    }
}
