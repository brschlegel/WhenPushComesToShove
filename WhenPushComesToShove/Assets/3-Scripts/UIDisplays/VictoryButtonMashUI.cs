using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class VictoryButtonMashUI : UIDisplay
{
    //[SerializeField] private float timeForMashing = 5;
    [SerializeField] private PlayerRankDisplay rankingDisplay;
    [SerializeField] private MinigameData data;
    private List<int> tiedIndexes = new List<int>();
    private List<PlayerInputHandler> tiedPlayers = new List<PlayerInputHandler>();
    [SerializeField] private Transform[] tiedPlayerDisplays = new Transform[4];
    [SerializeField] private Image sword;
    [SerializeField] private Image rune;
    [SerializeField] private Material[] playerColors = new Material[4];
    [SerializeField] private Material[] swordOutlines = new Material[4];
    [SerializeField] private Color[] swordGlowColors = new Color[4];
    [SerializeField] private ThresholdEndCondition threshold;
    [SerializeField] private TextMeshProUGUI tieText;
    [SerializeField] private float swordEndY;
    [SerializeField] private float swordOutOfStonePositionY = -4;
    private float swordTransformIncrement;
    private Vector3 swordStartPosition;
    [SerializeField] private float timeBetweenDisplays = 3;

    [SerializeField] private Transform confettiSpawnParent;
    private VictoryUILoserPortrait losingPortraits;
    public GameObject confetti;

    public override void HideDisplay()
    {
        gameObject.SetActive(false);
        rankingDisplay.ShowDisplay();
    }

    public override void ShowDisplay()
    {

        tiedIndexes = CheckForTies();

        if (tiedIndexes.Count < 2)
        {
            tieText.gameObject.SetActive(false);
        }

        //Display UI based on team size 
        tiedPlayerDisplays[tiedIndexes.Count - 1].gameObject.SetActive(true);

        //Losing Portraits
        losingPortraits = tiedPlayerDisplays[tiedIndexes.Count - 1].GetComponentInChildren<VictoryUILoserPortrait>();

        RawImage[] portraits = tiedPlayerDisplays[tiedIndexes.Count - 1].transform.GetChild(1).GetComponentsInChildren<RawImage>();

        for (int i = 0; i < portraits.Length; i++)
        {
            //portraits[i].sprite = PlayerConfigManager.Instance.playerPortraits[tiedIndexes[i]];
            portraits[i].material = playerColors[tiedIndexes[i]];
        }

        int losingPortraitTracker = 0;

        //Enable each players button mashing
        foreach (Transform player in GameState.players)
        {
            PlayerInputHandler handler = player.GetComponentInChildren<PlayerInputHandler>();

            if (tiedIndexes.Contains(handler.playerConfig.PlayerIndex))
            {
                //handler.EnableButonMashing();
                handler.onSelect += ButtonMashed;
                tiedPlayers.Add(handler);
            }
            else
            {
                handler.onSelect += losingPortraits.ThrowConfetti;
                losingPortraits.SetMaterial(losingPortraitTracker, playerColors[handler.playerConfig.PlayerIndex], handler.playerConfig.PlayerIndex);
                losingPortraitTracker++;
            }
        }

        MinigameData.onScoreAdded += ChangeSwordColor;
        gameObject.SetActive(true);
        StartCoroutine(WaitForTimeEnd());
    }

    private List<int> CheckForTies()
    {
        int highestScoreIndex = 0;
        List<int> tiedIndexes = new List<int>();

        //Find Winning Team
        for (int i = 0; i < GameState.playerScores.Length; i++)
        {
            if (GameState.playerScores[i] > GameState.playerScores[highestScoreIndex])
            {
                highestScoreIndex = i;
                tiedIndexes.Clear();
                tiedIndexes.Add(i);
            }
            else if (GameState.playerScores[i] == GameState.playerScores[highestScoreIndex])
            {
                tiedIndexes.Add(i);
            }
        }

        return tiedIndexes;
    }

    private IEnumerator WaitForTimeEnd()
    {
        yield return new WaitUntil(() => threshold.TestCondition());

        //Disable each players button mashing
        foreach (PlayerInputHandler handler in tiedPlayers)
        {
            //handler.DisableButtonMashing();
            handler.onSelect -= ButtonMashed;
        }

        MinigameData.onScoreAdded -= ChangeSwordColor;

        DisplaySwordForWinner();

        yield return new WaitForSeconds(timeBetweenDisplays);

        data.OnMinigameEnd(false);
        HideDisplay();
    }

    private void ButtonMashed(int playerIndex)
    {
        data.AddScoreForTeam(playerIndex, 1);
    }

    private void ChangeSwordColor(int teamIndex, float scoreToSetAs)
    {
        int index = data.GetHighestScoreIndex();
        sword.material = swordOutlines[index];
        rune.material = swordOutlines[index];
        Image glow = sword.transform.GetChild(0).GetComponent<Image>();
        glow.color = swordGlowColors[index];

        if (swordStartPosition == Vector3.zero)
        {
            swordStartPosition = sword.rectTransform.position;
            swordTransformIncrement = Mathf.Abs(swordEndY - swordStartPosition.y) / threshold.threshold;
        }

        //Update Position
        sword.rectTransform.position = new Vector3(swordStartPosition.x, swordStartPosition.y + (swordTransformIncrement * data.scores[index]), swordStartPosition.z);
    }

    private void DisplaySwordForWinner()
    {
        //Move sword out of stone
        sword.transform.DOMoveY(swordOutOfStonePositionY, .5f);
        //sword.rectTransform.position = new Vector3(swordStartPosition.x, swordOutOfStonePositionY, swordStartPosition.z);

        int index = data.GetHighestScoreIndex();
        PlayerComponentReferences references = GameState.players[index].GetComponent<PlayerComponentReferences>();

        confettiSpawnParent.gameObject.SetActive(true);

        //for (int i = 0; i < confettiSpawnParent.childCount; i++)
        //{
        //    GameObject confetti = Instantiate(references.confettiVFX, confettiSpawnParent.GetChild(i));
        //    confetti.transform.localScale = new Vector3(20, 20, 20);
        //}

        ParticleSystem[] vfx = confettiSpawnParent.GetComponentsInChildren<ParticleSystem>();
        
        //Spawn Confetti in Player's Color
        foreach (ParticleSystem confetti in vfx)
        {
            ParticleSystem.MainModule settings = confetti.main;
            settings.startColor = references.confettiVFX.GetComponent<ParticleSystem>().main.startColor;
        }
    }
}
