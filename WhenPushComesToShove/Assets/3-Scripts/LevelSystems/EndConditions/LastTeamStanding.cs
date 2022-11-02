using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LastTeamStanding : BaseEndCondition
{
    List<PlayerConfiguration> players = new List<PlayerConfiguration>();
    List<PlayerConfiguration> playersToRemove = new List<PlayerConfiguration>();

    [SerializeField] private bool showWinnerText = true;
    private TextMeshProUGUI winnerText;

    protected void OnEnable()
    {
        if(winnerText == null)
        {
            winnerText = UIManager.instance.victoryText;
            winnerText.gameObject.SetActive(false);
        }

        players = PlayerConfigManager.Instance.GetPlayerTeams();
        
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        winnerText.gameObject.SetActive(false);
        players.Clear();
        playersToRemove.Clear();

    }

    protected override void TestCondition()
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
                Debug.Log("Doesn't match");
                return;
            }

            Debug.Log(players[i].TeamIndex);
               
        }

        if (showWinnerText)
            DisplayWinner(players[0].TeamIndex + 1);

        base.TestCondition();
    }

    private void DisplayWinner(int teamNum)
    {
        winnerText.gameObject.SetActive(true);
        winnerText.text = "Team " + teamNum + " Won!";
    }
}
