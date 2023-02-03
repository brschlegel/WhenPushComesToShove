using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayUntilPlayerJoins : UIDisplay
{
    public TextMeshProUGUI explanationText;

    public override void ShowDisplay()
    {
        isDone = false;
        gameObject.SetActive(true);
        explanationText.gameObject.SetActive(true);
        StartCoroutine(Delay());
    }

    public override void HideDisplay()
    {
        isDone = true;
        explanationText.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private IEnumerator Delay()
    {
        yield return new WaitUntil(() => PlayerConfigManager.Instance.GetPlayerConfigs().Count > 0);
        HideDisplay();
    }
}
