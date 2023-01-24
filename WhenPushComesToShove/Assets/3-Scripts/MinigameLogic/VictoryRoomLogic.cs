using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VictoryRoomLogic : MinigameLogic
{
    public TextMeshProUGUI winnerText;
    public PlayerRankDisplay rankDisplay;

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

        base.Init();
    }
}
