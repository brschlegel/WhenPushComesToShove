using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class VictoryRoomLogic : MinigameLogic
{
    public TextMeshProUGUI winnerText;
    public PlayerRankDisplay rankDisplay;

    Transform winningPlayer;

    private void OnEnable()
    {
        startingUIDisplay.ShowDisplay();
        endCondition.Init();
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

        //winningPlayer.GetComponent<PlayerComponentReferences>().crownIcon.gameObject.SetActive(true);

        base.Init();
    }

    public override void CleanUp()
    {
        GameState.lastGameWinnerIndex = rankDisplay.playerRankOrder[0];
        winningPlayer = GameState.players[rankDisplay.playerRankOrder[0]];
        //winningPlayer.GetComponent<PlayerComponentReferences>().crownIcon.gameObject.SetActive(false);
        //GameObject.FindWithTag("ModifierManager").GetComponent<ModifierManager>().RemoveAllModifiers();
        base.CleanUp();
    }
}
