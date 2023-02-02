using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPortrait : MonoBehaviour
{
   public RawImage portrait;
   public RawImage checkmark;
   private bool ready = false;
   private bool visible = false;
    [SerializeField] private Transform crown;

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

    public void DisplayCrown()
    {
        crown.gameObject.SetActive(true);
    }

}
