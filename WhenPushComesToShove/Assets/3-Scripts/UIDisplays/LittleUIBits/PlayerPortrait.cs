using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPortrait : MonoBehaviour
{
    public Image portrait;
    public RawImage checkmark;
    public int playerIndex = -1;
    private bool ready = false;
    private bool visible = false;
    [SerializeField] private Transform crown;
    public Material playerColor;

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

}
