using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VictoryRoomLogic : MinigameLogic
{
    public TextMeshProUGUI winnerText;
    public PlayerRankDisplay rankDisplay;

    Transform winningPlayer;

    private void OnEnable()
    {
        startingUIDisplay.ShowDisplay();
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
        //rankDisplay.ShowDisplay();

        //winningPlayer = GameState.players[rankDisplay.playerRankOrder[0]];

        //winningPlayer.GetComponentInChildren<PlayerInputHandler>().crownBox.SetActive(true);

        base.Init();
    }

    public override void CleanUp()
    {
        winningPlayer.GetComponentInChildren<PlayerInputHandler>().crownBox.SetActive(false);

        base.CleanUp();
    }
}
