using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyLogic : MinigameLogic
{
    [SerializeField] private Sprite swordSprite;

    public void OnDisable()
    {
        if (GameState.players.Count > 0 && GameState.lastGameWinnerIndex != -1)
        {
            PlayerComponentReferences references = GameState.players[GameState.lastGameWinnerIndex].GetComponent<PlayerComponentReferences>();

            references.sword.gameObject.SetActive(false);
        }
    }

    public override void Init()
    {
        startingUIDisplay.ShowDisplay();

        if (GameState.players.Count > 0 && GameState.lastGameWinnerIndex != -1)
        {
            PlayerComponentReferences references = GameState.players[GameState.lastGameWinnerIndex].GetComponent<PlayerComponentReferences>();

            references.sword.gameObject.SetActive(true);
            references.sword.sprite = swordSprite;
            Transform swordGlow = references.sword.transform.GetChild(0);
            swordGlow.GetComponent<SpriteRenderer>().color = PlayerConfigManager.Instance.playerColors[GameState.lastGameWinnerIndex];
        }
    }
}
