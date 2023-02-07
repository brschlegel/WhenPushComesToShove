using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseModifier : MonoBehaviour
{
    [HideInInspector]
    public Transform currentGameRoot;

    
    public List<Minigame> minigamesAffected;
    public Sprite icon;
    public RuntimeAnimatorController iconAnimator;
    public string name;
    public string description;

    //These aren't abstract so that you don't  have to include them
    public virtual void GameInit() {}

    public virtual void GameStart() {}

    public virtual void Init() {}
    public virtual void CleanUp() {}
}
