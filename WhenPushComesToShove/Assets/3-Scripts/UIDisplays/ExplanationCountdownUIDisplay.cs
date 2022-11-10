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
        gameObject.SetActive(true);
       countdown.gameObject.SetActive(true);
       explanationText.gameObject.SetActive(true);
       countdown.onCountdownEnded += HideDisplay;
    }

    public override void HideDisplay()
    {
        isDone = true;
        countdown.gameObject.SetActive(false);
        explanationText.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

}
