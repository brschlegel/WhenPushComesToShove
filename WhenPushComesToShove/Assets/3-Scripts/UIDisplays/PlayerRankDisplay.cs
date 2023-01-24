using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class PlayerRankDisplay : UIDisplay
{
    [SerializeField] private TextMeshProUGUI winnerText;
    [SerializeField] private TextMeshProUGUI rankText;

    public override void HideDisplay()
    {
        gameObject.SetActive(false);
    }

    public override void ShowDisplay()
    {
        int[] playerOrder = GetRankOrder();

        winnerText.text = GameState.playerNames[playerOrder[0]] + " player won!";

        rankText.text = "2nd: " + GameState.playerNames[playerOrder[1]] + 
            "\n3rd: " + GameState.playerNames[playerOrder[2]] + 
            "\n4th: " + GameState.playerNames[playerOrder[3]];

        gameObject.SetActive(true);
    }

    private int[] GetRankOrder()
    {
        //Pass in values and sort highest to lowest
        int[] sortedScoreArray = new int[4];

        for (int i = 0; i < GameState.playerScores.Length; i++)
        {
            sortedScoreArray[i] = GameState.playerScores[i];
        }


        Array.Sort(sortedScoreArray);
        Array.Reverse(sortedScoreArray);

        int[] playerIndexOrder = new int[sortedScoreArray.Length];

        for (int i = 0; i < sortedScoreArray.Length; i++)
        {
            for (int j = 0; j < GameState.playerScores.Length; j++)
            {
                //If the scores match and that index is not already recorded
                if (sortedScoreArray[i] == GameState.playerScores[j] && !Array.Exists(playerIndexOrder, s => s == j))
                {
                    playerIndexOrder[i] = j;
                    break;
                }
            }
        }

        foreach  (int p in playerIndexOrder)
        {
            Debug.Log(p);
        }

        return playerIndexOrder;
    }
}
