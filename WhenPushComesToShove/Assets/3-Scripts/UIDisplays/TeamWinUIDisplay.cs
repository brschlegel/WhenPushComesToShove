using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TeamWinUIDisplay : UIDisplay
{
    [HideInInspector]
    public int winningTeamNum;

    [SerializeField]
    private TextMeshProUGUI victoryText;
    [SerializeField]
    private float delay = 1;
    [SerializeField] Transform[] teamWinDisplays = new Transform[2];
    [SerializeField] Transform[] tiedDisplays = new Transform[4];

    public override void ShowDisplay()
    {
        isDone = false;
        gameObject.SetActive(true);
        victoryText.gameObject.SetActive(true);

        if (winningTeamNum == -1)
        {
            victoryText.text = "Tie!";
            ShowTieDisplay();
        }
        else
        {
            //Find All Winning Players and Add them to the Text
            string winningPlayers = "";
            List<int> winningPlayerIndexs = new List<int>();
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

                    winningPlayerIndexs.Add(config.PlayerIndex);
                }
            }

            ShowTeamDisplay(winningPlayerIndexs);
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

    public void ShowTieDisplay()
    {
        tiedDisplays[GameState.players.Count - 1].gameObject.SetActive(true);
    }

    public void ShowTeamDisplay(List<int> winningIndexes)
    {
        Transform display = teamWinDisplays[winningIndexes.Count - 1];
        display.gameObject.SetActive(true);

        for (int i = 0; i < display.childCount; i++)
        {
            RawImage image = display.GetChild(i).GetComponent<RawImage>();
            image.material = PlayerConfigManager.Instance.playerOutlines[winningIndexes[i]];
        }
    }
}
