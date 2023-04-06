using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    public static FMODEvents instance;

    #region Event References
    [Header("BGM")]
    public EventReference bgm;

    [Header("Shove SFX")]
    public EventReference lightShove;
    public EventReference heavyShove;
    public EventReference missShove;

    [Header("Dash")]
    public EventReference dash;

    [Header("Props")]
    public EventReference bombExplosion;

    [Header("UI")]
    public EventReference readyUp;
    #endregion

    private void Awake()
    {
        if (instance != null)
            Debug.LogError("Found more than one FMODEvents instance in the scene");

        instance = this;
    }
}
