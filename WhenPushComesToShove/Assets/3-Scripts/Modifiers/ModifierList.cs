using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MinigameList
{
    public GameObject modifierPrefab;
    public List<Minigame> minigamesAffected;

    public Minigame this[int key]
    {
        get
        {
            return minigamesAffected[key];
        }
        set
        {
            minigamesAffected[key] = value;
        }
    }
}

[CreateAssetMenu(menuName = "Modifiers/Modifier List")]
public class ModifierList : ScriptableObject
{
    public List<MinigameList> modifiers;
}
