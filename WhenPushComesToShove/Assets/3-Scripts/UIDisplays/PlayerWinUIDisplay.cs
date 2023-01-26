using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerWinUIDisplay : UIDisplay
{

    [HideInInspector]
    public string winnerName;
    [SerializeField]
    private float delay;
    [SerializeField]
    private TextMeshProUGUI text;

    public override void ShowDisplay()
    {
        text.text = winnerName + " player is the winner!";
        text.gameObject.SetActive(true);
        isDone = false;
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
        text.gameObject.SetActive(false);
    }
}
