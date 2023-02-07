using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class PlayerRankDisplay : UIDisplay
{
    [SerializeField] private TextMeshProUGUI winnerText;
    [HideInInspector] public int[] playerRankOrder;
    [HideInInspector] public int[] scoresInOrder;
    [SerializeField] private Image winnerPortrait;
    [SerializeField] private Transform[] rankUIs = new Transform[3];

    public override void HideDisplay()
    {
        gameObject.SetActive(false);
    }

    public override void ShowDisplay()
    {
        playerRankOrder = GetRankOrder();

        winnerText.text = GameState.playerNames[playerRankOrder[0]] + " player won! - " + scoresInOrder[0];
        winnerPortrait.sprite = PlayerConfigManager.Instance.playerPortraits[playerRankOrder[0]];

        int numOfPlayers = GameState.players.Count;

        for (int i = 1; i < numOfPlayers; i++)
        {
            rankUIs[i - 1].gameObject.SetActive(true);

            TextMeshProUGUI text = rankUIs[i - 1].GetComponentInChildren<TextMeshProUGUI>();
            text.text = scoresInOrder[i].ToString();
            rankUIs[i - 1].GetComponentInChildren<Image>().sprite = PlayerConfigManager.Instance.playerPortraits[playerRankOrder[i]];
        }

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
                    if (j < GameState.players.Count)
                    {
                        playerIndexOrder[i] = j;
                        break;
                    }
                }
            }
        }

        scoresInOrder = sortedScoreArray;

        foreach  (int p in playerIndexOrder)
        {
            Debug.Log(p);
        }

        return playerIndexOrder;
    }
}
