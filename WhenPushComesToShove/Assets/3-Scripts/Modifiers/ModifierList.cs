using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ModifierSettings
{
    public GameObject modifierPrefab;
}

[CreateAssetMenu(menuName = "Modifiers/Modifier List")]
public class ModifierList : ScriptableObject
{
    public List<ModifierSettings> modifiers;
}
