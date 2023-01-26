using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VictoryRoomLogic : MinigameLogic
{
    public TextMeshProUGUI winnerText;
    public PlayerRankDisplay rankDisplay;
    public Sprite playerCrown;
    Transform winningPlayer;

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
        rankDisplay.ShowDisplay();

        winningPlayer = GameState.players[rankDisplay.playerRankOrder[0]];

        winningPlayer.GetChild(13).gameObject.SetActive(true);

        base.Init();
    }

    public override void CleanUp()
    {
        winningPlayer.GetChild(13).gameObject.SetActive(false);

        base.CleanUp();
    }
}
