using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUI : UIDisplay
{
    public TextMeshProUGUI[] scores = new TextMeshProUGUI[4];

    public void OnEnable()
    {
        Init();
    }

    public void Init()
    {
        MinigameData.onScoreAdded += UpdateScore;
    }

    public override void HideDisplay()
    {
        gameObject.SetActive(false);
    }

    public override void ShowDisplay()
    {
        gameObject.SetActive(true);
    }

    public void UpdateScore(int index, int scoreToSetAs)
    {
        if (scores[index] == null)
        {
            Debug.LogError("No score UI set for this index");
        }

        scores[index].text = scoreToSetAs.ToString();
    }

    public void OnDisable()
    {
        MinigameData.onScoreAdded -= UpdateScore;
    }
}
