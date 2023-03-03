using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MinigameData : MonoBehaviour
{
    public float[] scores = new float[4];
    public static Action<int, float> onScoreAdded;

    private bool canUpdateScore = true;

    public void AddScoreForTeam(int teamIndex, float scoreToAdd)
    {
        if (!canUpdateScore)
        {
            return;
        }

        scores[teamIndex] += scoreToAdd;

        if (onScoreAdded != null)
        {
            onScoreAdded.Invoke(teamIndex, scores[teamIndex]);
        }
    }

    public void RemoveScoreForTeam(int teamIndex, int scoreToRemove)
    {
        if (!canUpdateScore)
        {
            return;
        }

        scores[teamIndex] -= scoreToRemove;
    }

    public void OnMinigameEnd(bool useTeamIndex = true)
    {
        canUpdateScore = false;

        int highestScoreIndex = GetHighestScoreIndex();

        //Update Gamestate with winners
        if (useTeamIndex)
        {
            List<PlayerConfiguration> players = PlayerTeamFormations.instance.GetPlayerTeams();
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].TeamIndex == highestScoreIndex)
                {
                    GameState.playerScores[players[i].PlayerIndex] += 1;
                }
            }
        }
        else
        {
            GameState.playerScores[highestScoreIndex] += 1;
        }

        //Cleanup just in case
        scores = new float[4];
    }

    public int GetHighestScoreIndex()
    {
        int highestScoreIndex = 0;

        //Find Winning Team
        for (int i = 1; i < scores.Length; i++)
        {
            if (scores[i] > scores[highestScoreIndex])
            {
                highestScoreIndex = i;
            }

            Debug.Log(scores[i]);
        }
        return highestScoreIndex;
    }
}
