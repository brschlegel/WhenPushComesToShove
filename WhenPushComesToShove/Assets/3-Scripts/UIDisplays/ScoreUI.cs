using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUI : UIDisplay
{
    public TextMeshProUGUI[] scores = new TextMeshProUGUI[4];
    public bool displayBasedOnPlayerNum = false;
    [SerializeField] private Transform[] scoreUIs = new Transform[4];

    public void OnEnable()
    {
        Init();
    }

    public void Init()
    {
        if (displayBasedOnPlayerNum)
        {
            for (int i = GameState.players.Count; i < PlayerConfigManager.Instance.GetMaxPlayers(); i++)
            {
                scoreUIs[i].gameObject.SetActive(false);
            }
        }

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

    public void UpdateScore(int index, float scoreToSetAs)
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
