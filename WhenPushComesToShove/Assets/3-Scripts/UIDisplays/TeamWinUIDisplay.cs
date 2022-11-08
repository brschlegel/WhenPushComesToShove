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
    private float delay;

    public override void ShowDisplay()
    {
        isDone = false;
        victoryText.gameObject.SetActive(true);
        victoryText.text = "Team " + winningTeamNum + " Won!";
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
