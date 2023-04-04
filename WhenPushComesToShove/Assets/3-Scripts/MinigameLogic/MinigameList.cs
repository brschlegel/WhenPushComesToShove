using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MinigameSettings
{
    public GameObject minigamePrefab;
}

[CreateAssetMenu(menuName = "Minigames/Minigame List")]
public class MinigameList : ScriptableObject
{
    public List<MinigameSettings> minigames;
}
