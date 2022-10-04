using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    //UI References
    public TextMeshProUGUI roomText;
    public TextMeshProUGUI victoryText;

    private void Awake()
    {
        if (instance != null)
        {
            Init();
        }
    }

    public void Init()
    {
        instance = this;
    }
}
