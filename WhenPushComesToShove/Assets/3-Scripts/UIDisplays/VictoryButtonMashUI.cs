using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryButtonMashUI : UIDisplay
{
    [SerializeField] private float timeForMashing = 5;
    [SerializeField] private PlayerRankDisplay rankingDisplay;
    [SerializeField] private MinigameData data;
    private List<int> tiedIndexes = new List<int>();
    private List<PlayerInputHandler> tiedPlayers = new List<PlayerInputHandler>();
    [SerializeField] private Transform[] tiedPlayerDisplays = new Transform[4];
    [SerializeField] private RawImage sword;
    [SerializeField] private Color[] playerColors = new Color[4];
    [SerializeField] private ThresholdEndCondition threshold;
    //[SerializeField] private Sprite[] playerPortraitSprites = new Sprite[4];

    public override void HideDisplay()
    {
        gameObject.SetActive(false);
        rankingDisplay.ShowDisplay();
    }

    public override void ShowDisplay()
    {
        tiedIndexes = CheckForTies();

        //Display UI based on team size 
        tiedPlayerDisplays[tiedIndexes.Count - 1].gameObject.SetActive(true);

        RawImage[] portraits = tiedPlayerDisplays[tiedIndexes.Count - 1].transform.GetChild(1).GetComponentsInChildren<RawImage>();

        for (int i = 0; i < portraits.Length; i++)
        {
            //portraits[i].sprite = PlayerConfigManager.Instance.playerPortraits[tiedIndexes[i]];
            portraits[i].material = PlayerConfigManager.Instance.playerOutlines[tiedIndexes[i]];
        }

        //Enable each players button mashing
        foreach (int index in tiedIndexes)
        {
            PlayerInputHandler handler = GameState.players[index].GetComponentInChildren<PlayerInputHandler>();
            //handler.EnableButonMashing();
            handler.onSelect += ButtonMashed;
            tiedPlayers.Add(handler);
        }

        MinigameData.onScoreAdded += ChangeSwordColor;
        gameObject.SetActive(true);
        StartCoroutine(WaitForTimeEnd());
    }

    private List<int> CheckForTies()
    {
        int highestScoreIndex = 0;
        List<int> tiedIndexes = new List<int>();

        //Find Winning Team
        for (int i = 0; i < GameState.playerScores.Length; i++)
        {
            if (GameState.playerScores[i] > GameState.playerScores[highestScoreIndex])
            {
                highestScoreIndex = i;
                tiedIndexes.Clear();
                tiedIndexes.Add(i);
            }
            else if (GameState.playerScores[i] == GameState.playerScores[highestScoreIndex])
            {
                tiedIndexes.Add(i);
            }
        }

        return tiedIndexes;
    }

    private IEnumerator WaitForTimeEnd()
    {
        yield return new WaitUntil(() => threshold.TestCondition());

        //Enable each players button mashing
        foreach (PlayerInputHandler handler in tiedPlayers)
        {
            //handler.DisableButtonMashing();
            handler.onSelect -= ButtonMashed;
        }

        MinigameData.onScoreAdded -= ChangeSwordColor;

        data.OnMinigameEnd(false);
        HideDisplay();
    }

    private void ButtonMashed(int playerIndex)
    {
        data.AddScoreForTeam(playerIndex, 1);
    }

    private void ChangeSwordColor(int teamIndex, int scoreToSetAs)
    {
        int index = data.GetHighestScoreIndex();
        sword.material = PlayerConfigManager.Instance.playerOutlines[index];
    }
}
