using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExplanationCountdownUIDisplay : UIDisplay
{
    [SerializeField]
    private Countdown countdown;
    [SerializeField]
    private TextMeshProUGUI explanationText;

    public override void ShowDisplay()
    {
        isDone = false;
        gameObject.SetActive(true);
       countdown.gameObject.SetActive(true);
       explanationText.gameObject.SetActive(true);
       countdown.onCountdownEnded += HideDisplay;

      foreach (Transform p in GameState.players)
      {
            p.GetComponentInChildren<PlayerMovementScript>().ChangeMoveSpeed(0);
      }
    }

    public override void HideDisplay()
    {
        foreach (Transform p in GameState.players)
        {
            p.GetComponentInChildren<PlayerMovementScript>().ResetMoveSpeed();
        }

        isDone = true;
        countdown.gameObject.SetActive(false);
        explanationText.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

}
