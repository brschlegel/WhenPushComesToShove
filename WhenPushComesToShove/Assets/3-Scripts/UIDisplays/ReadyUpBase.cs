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
    private Transform portraitParent;
    [SerializeField]
    private Transform readyParent;
    [SerializeField]
    private Countdown countdown;

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
        for(int i = 0; i < 4; i++)
        {
            portraits.Add(portraitParent.GetChild(i).GetComponent<PlayerPortrait>());
            portraits[i].Visible = i < numPlayers;
            
        }
        //use game state

        foreach(Transform p in GameState.players)
        {
            p.GetComponentInChildren<PlayerInputHandler>().onSelect += ReadyUp;
        }
    }

    public void ReadyUp(int index)
    {
        portraits[index].Ready = true;
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
}
