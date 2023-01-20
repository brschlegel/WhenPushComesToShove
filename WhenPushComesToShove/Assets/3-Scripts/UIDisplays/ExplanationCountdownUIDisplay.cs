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
        explanationText.gameObject.SetActive(true);

        if (countdown != null)
        {
            countdown.gameObject.SetActive(true);
            countdown.onCountdownEnded += HideDisplay;
        }
    }

    public override void HideDisplay()
    {
        isDone = true;
        explanationText.gameObject.SetActive(false);
        gameObject.SetActive(false);

        if (countdown != null)
        {
            countdown.gameObject.SetActive(false);
        }
    }

}
