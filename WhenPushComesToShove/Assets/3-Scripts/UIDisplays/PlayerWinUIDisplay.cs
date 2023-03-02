using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerWinUIDisplay : UIDisplay
{

    [HideInInspector]
    public string winnerName;
    [SerializeField]
    private float delay;
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private Transform[] victoryDisplays = new Transform[4];
    [HideInInspector]
    public bool tie = false;
    [HideInInspector]
    public List<int> tiedIndexes = new List<int>();

    public override void ShowDisplay()
    {
        gameObject.SetActive(true);

        DisplaySetup();

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

    public void DisplaySetup()
    {
        if (tie == false)
            text.text = winnerName + " player is the winner!";
        else
        {
            text.text = "Tie between " + winnerName;
        }

        Transform display = victoryDisplays[tiedIndexes.Count - 1];
        display.gameObject.SetActive(true);

        //Loop Through Children and Assign Materials
        for (int i = 0; i < display.childCount; i++)
        {
            RawImage image = display.GetChild(i).GetComponent<RawImage>();
            image.material = PlayerConfigManager.Instance.playerOutlines[tiedIndexes[i]];
        }
    }
}
