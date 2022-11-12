using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MinigameData : MonoBehaviour
{
    public int[] scores = new int[4];
    public static Action<int, int> onScoreAdded;

    public void AddScoreForTeam(int teamIndex, int scoreToAdd)
    {
        scores[teamIndex] += scoreToAdd;
        onScoreAdded.Invoke(teamIndex, scores[teamIndex]);
    }

    public void OnMinigameEnd()
    {
        int highestScoreIndex = 0;

        //Find Winning Team
        for (int i = 1; i < scores.Length; i++)
        {
            if (scores[i] > scores[highestScoreIndex])
            {
                highestScoreIndex = i;
            }
        }

        //Update Gamestate with winners
        List<PlayerConfiguration> players = PlayerConfigManager.Instance.GetPlayerTeams();
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].TeamIndex == highestScoreIndex)
            {
                GameState.playerScores[players[i].PlayerIndex] += 1;
            }
        }

        //Cleanup just in case
        scores = new int[4];
    }
}
