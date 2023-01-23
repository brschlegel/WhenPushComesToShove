using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VictoryRoomLogic : MinigameLogic
{
    public TextMeshProUGUI winnerText;

    public void OnEnable()
    {
        Init();
    }

    public void Update()
    {
        if (endCondition.TestCondition())
        {
            EndGame();
        }
    }

    public override void Init()
    {
        int highestScoreIndex = 0;

        //Find Winning Team
        for (int i = 1; i < GameState.playerScores.Length; i++)
        {
            if (GameState.playerScores[i] > GameState.playerScores[highestScoreIndex])
            {
                highestScoreIndex = i;
            }
        }

        PlayerConfiguration winningPlayer = GameState.players[highestScoreIndex].GetComponentInChildren<PlayerInputHandler>().playerConfig;        
        winnerText.text = GameState.playerNames[winningPlayer.PlayerIndex] + " player won!";

        base.Init();
    }
}
