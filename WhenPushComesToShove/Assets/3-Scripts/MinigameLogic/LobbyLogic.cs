using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyLogic : MinigameLogic
{

    public void OnDisable()
    {
        if (GameState.players.Count > 0 && GameState.lastGameWinnerIndex != -1)
        {
            PlayerInputHandler handler = GameState.players[GameState.lastGameWinnerIndex].GetComponentInChildren<PlayerInputHandler>();

            handler.sword.gameObject.SetActive(false);
        }
    }

    public override void Init()
    {
        if (GameState.players.Count > 0 && GameState.lastGameWinnerIndex != -1)
        {
            PlayerInputHandler handler = GameState.players[GameState.lastGameWinnerIndex].GetComponentInChildren<PlayerInputHandler>();

            handler.sword.gameObject.SetActive(true);
            handler.sword.GetComponentInChildren<SpriteRenderer>().color = PlayerConfigManager.Instance.playerColors[handler.playerConfig.PlayerIndex];
        }
    }
}
