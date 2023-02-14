using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ReadyUpBase : UIDisplay
{
    [SerializeField]
    private TextMeshProUGUI gameNameText;
    [SerializeField]
    private TextMeshProUGUI gameDescriptionText;
    [SerializeField]
    private Transform portraitParent;
    [SerializeField]
    private Transform readyParent;
    [SerializeField]
    private Countdown countdown;

    [SerializeField]
    private TextMeshProUGUI vs2v2;
    [SerializeField]
    private TextMeshProUGUI vs1v3;

    private int numPlayers;
    private List<PlayerPortrait> portraits;
    private float delay = 1;
    private float countdownTime = 3;

    public override void ShowDisplay()
    {

        numPlayers = GameState.players.Count;
        portraits = new List<PlayerPortrait>();
        gameObject.SetActive(true);
        isDone = false;

        //Find highest ranking player
        int highestScoreIndex = 0;

        List<int> tiedIndexes = new List<int>();

        //Find Winning Team
        for (int i = 1; i < GameState.playerScores.Length; i++)
        {
            if (GameState.playerScores[i] > GameState.playerScores[highestScoreIndex])
            {
                highestScoreIndex = i;
                tiedIndexes.Clear();
            }
            else if (GameState.playerScores[i] == GameState.playerScores[highestScoreIndex])
            {
                tiedIndexes.Add(i);
            }
        }

        for (int i = 0; i < 4; i++)
        {
            portraits.Add(portraitParent.GetChild(i).GetComponent<PlayerPortrait>());
            //portraits[i].Visible = i < numPlayers;

            if (portraits[i].Visible && (i == highestScoreIndex || tiedIndexes.Contains(i)))
            {
                if (GameState.playerScores[highestScoreIndex] > 0)
                {
                    portraits[i].DisplayCrown();
                }
            }
            
        }
        //use game state

        ShowBasedOnTeams();

        foreach (Transform p in GameState.players)
        {
            p.GetComponentInChildren<PlayerInputHandler>().onSelect += ReadyUp;
        }
    }

    public void ReadyUp(int index)
    {
        foreach (PlayerPortrait p in portraits)
        {
            if (p.playerIndex == index)
            {
                p.Ready = true;
            }
        }

        //portraits[index].Ready = true;
        if (CheckIsAllReady())
        {
            StartCoroutine(WaitToCount());
        }
    }

    //Checks if every player is ready
    private bool CheckIsAllReady()
    {
        foreach (PlayerPortrait p in portraits)
        {
            if (!p.Ready && p.Visible)
            {
                return false;
            }
        }
        return true;
    }

    public override void HideDisplay()
    {
        gameObject.SetActive(false);
        isDone = true;
        foreach(Transform p in GameState.players)
        {
            p.GetComponentInChildren<PlayerInputHandler>().onSelect -= ReadyUp;
        }
    }

    private IEnumerator WaitToCount()
    {
        yield return new WaitForSeconds(delay);
        StartCounting();
    }

    private void StartCounting()
    {
        readyParent.gameObject.SetActive(false);
        countdown.countdownTime = countdownTime;
        countdown.gameObject.SetActive(true);
        countdown.onCountdownEnded += HideDisplay;
    }

    private void ShowBasedOnTeams()
    {

        switch (GameState.currentRoomType)
        {
            case LevelType.Lobby:
                break;
            case LevelType.Dungeon:
                break;
            case LevelType.Arena:
                for (int i = 0; i < portraits.Count; i++)
                {
                    portraits[i].Visible = i < numPlayers;
                    portraits[i].playerIndex = i;
                }
                break;
            case LevelType.TwoTwo:
                int leftSideCount = 0;
                int rightSideCount = 0;

                foreach (PlayerPortrait p in portraits)
                {
                    p.Visible = false;
                }

                for (int i = 0; i < GameState.players.Count; i++)
                {
                    PlayerInputHandler handler = GameState.players[i].GetComponentInChildren<PlayerInputHandler>();
                    if (handler.playerConfig.TeamIndex < 1)
                    {
                        PlayerPortrait portrait = portraitParent.GetChild(0 + leftSideCount).GetComponent<PlayerPortrait>();
                        portrait.portrait.sprite = PlayerConfigManager.Instance.playerPortraits[i];
                        portrait.Visible = true;
                        portrait.playerIndex = i;
                        leftSideCount++;
                    }
                    else
                    {
                        PlayerPortrait portrait = portraitParent.GetChild(2 + rightSideCount).GetComponent<PlayerPortrait>();
                        portrait.portrait.sprite = PlayerConfigManager.Instance.playerPortraits[i];
                        portrait.Visible = true;
                        portrait.playerIndex = i;
                        rightSideCount++;
                    }
                }

                vs2v2.gameObject.SetActive(true);             
                break;
            case LevelType.ThreeOne:
                int teamSideCount = 0;

                foreach (PlayerPortrait p in portraits)
                {
                    p.Visible = false;
                }

                for (int i = 0; i < GameState.players.Count; i++)
                {
                    PlayerInputHandler handler = GameState.players[i].GetComponentInChildren<PlayerInputHandler>();
                    if (handler.playerConfig.TeamIndex < 1)
                    {
                        PlayerPortrait portrait = portraitParent.GetChild(0 + teamSideCount).GetComponent<PlayerPortrait>();
                        portrait.portrait.sprite = PlayerConfigManager.Instance.playerPortraits[i];
                        portrait.Visible = true;
                        portrait.playerIndex = i;
                        teamSideCount++;
                    }
                    else
                    {
                        PlayerPortrait portrait = portraitParent.GetChild(3).GetComponent<PlayerPortrait>();
                        portrait.portrait.sprite = PlayerConfigManager.Instance.playerPortraits[i];
                        portrait.Visible = true;
                        portrait.playerIndex = i;
                    }
                }

                vs1v3.gameObject.SetActive(true);
                break;
            case LevelType.Modifier:
                break;
            default:
                break;
        }
    }
}
