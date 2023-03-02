using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MinigameData : MonoBehaviour
{
    public float[] scores = new float[4];
    public static Action<int, float> onScoreAdded;

    private bool canUpdateScore = true;

    public void Init()
    {
        scores = new float[] { -1f, - 1f, -1f, -1f };

        if (GameState.currentRoomType == LevelType.Arena)
        {
            //Ensure only active players are counted in score
            foreach (Transform p in GameState.players)
            {
                //p.GetComponentInChildren<PlayerMovementScript>().ResetMoveSpeed();
                PlayerInputHandler handler = p.GetComponentInChildren<PlayerInputHandler>();
                AddScoreForTeam(handler.playerConfig.PlayerIndex, 1);
            }
        }
        else
        {
            AddScoreForTeam(0, 1);
            AddScoreForTeam(1, 1);
        }

    }

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

        List<int> highestScoreIndexes = GetHighestScoreWithTies();

        //Update Gamestate with winners
        if (useTeamIndex)
        {
            List<PlayerConfiguration> players = PlayerTeamFormations.instance.GetPlayerTeams();
            for (int i = 0; i < players.Count; i++)
            {
                foreach (int index in highestScoreIndexes)
                {
                    if (players[i].TeamIndex == index)
                    {
                        GameState.playerScores[players[i].PlayerIndex] += 1;
                    }
                }

            }
        }
        else
        {
            foreach (int index in highestScoreIndexes)
            {
                GameState.playerScores[index] += 1;
            }
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

    public List<int> GetHighestScoreWithTies()
    {
        int highestScoreIndex = 0;
        List<int> tiedIndexes = new List<int>();

        //Find Winning Team
        for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i] > scores[highestScoreIndex])
            {
                highestScoreIndex = i;
                tiedIndexes.Clear();
                tiedIndexes.Add(i);
            }
            else if (scores[i] == scores[highestScoreIndex])
            {
                tiedIndexes.Add(i);
            }
        }

        return tiedIndexes;
    }
}
