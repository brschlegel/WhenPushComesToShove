using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerPortrait : MonoBehaviour
{
    public Image portrait;
    public RawImage checkmark;
    public Image teamIcon;
    public int playerIndex = -1;
    private bool ready = false;
    private bool visible = false;
    [SerializeField] private Transform crown;
    public Material playerColor;
    [SerializeField] private Image flag;

    public void OnDisable()
    {
        crown.gameObject.SetActive(false);
    }

    public bool Ready
   {
        get {return ready;}
        set
        {
            ready = value;
            checkmark.gameObject.SetActive(value);
        }
   }

    public bool Visible
    {
        get {return visible;}
        set
        {
            visible = value;
            portrait.gameObject.SetActive(visible);
            flag.gameObject.SetActive(visible);
        }
    }

    public Material PlayerColor
    {
        get { return playerColor; }
        set 
        {
            Debug.Log("SETTING PLAYER COLOR");
            playerColor = value;
            portrait.GetComponent<Image>().material = playerColor;
        }
    }

    public void DisplayCrown()
    {
        crown.gameObject.SetActive(true);
    }

    public void SetUpScoreFlag(int score, Material color)
    {
        flag.material = color;
        TextMeshProUGUI scoretext = flag.GetComponentInChildren<TextMeshProUGUI>();
        scoretext.text = score.ToString();
    }

}
