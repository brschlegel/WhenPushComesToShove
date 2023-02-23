using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayUntilPlayerJoins : UIDisplay
{
    public TextMeshProUGUI explanationText;
    public TextMeshProUGUI textToDisplayUntilAllPlayersJoined;

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
        if (textToDisplayUntilAllPlayersJoined != null)
        {
            textToDisplayUntilAllPlayersJoined.gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
    }

    private IEnumerator Delay()
    {
        yield return new WaitUntil(() => PlayerConfigManager.Instance.GetPlayerConfigs().Count > 0);

        explanationText.gameObject.SetActive(false);

        if (textToDisplayUntilAllPlayersJoined != null)
        {
            yield return new WaitUntil(() => PlayerConfigManager.Instance.GetPlayerConfigs().Count >= PlayerConfigManager.Instance.GetMaxPlayers());
        }

        HideDisplay();
    }
}
