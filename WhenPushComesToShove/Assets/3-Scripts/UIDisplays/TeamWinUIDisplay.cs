using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TeamWinUIDisplay : UIDisplay
{
    [HideInInspector]
    public int winningTeamNum;

    [SerializeField]
    private TextMeshProUGUI victoryText;
    [SerializeField]
    private float delay = 1;

    public override void ShowDisplay()
    {
        isDone = false;
        victoryText.gameObject.SetActive(true);

        if (winningTeamNum == -1)
        {
            victoryText.text = "Tie!";
        }
        else
        {
            //Find All Winning Players and Add them to the Text
            string winningPlayers = "";

            foreach (Transform p in GameState.players)
            {
                PlayerConfiguration config = p.GetComponentInChildren<PlayerInputHandler>().playerConfig;
                if (config.TeamIndex == winningTeamNum)
                {
                    if (winningPlayers == "")
                    {
                        winningPlayers += GameState.playerNames[config.PlayerIndex];
                    }
                    else
                    {
                        winningPlayers += " and " + GameState.playerNames[config.PlayerIndex];
                    }
                }
            }

            victoryText.text = winningPlayers + " players won!";
        }
 
        CoroutineManager.StartGlobalCoroutine(WaitToFinish());
    }

    private IEnumerator WaitToFinish()
    {
        yield return new WaitForSeconds(delay);
        isDone = true;
        HideDisplay();
    }

    public override void HideDisplay()
    {
        victoryText.gameObject.SetActive(false);
    }
}
