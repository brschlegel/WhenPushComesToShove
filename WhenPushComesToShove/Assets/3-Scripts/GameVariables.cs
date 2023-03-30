using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Variables/Game Variables")]
public class GameVariables : ScriptableObject
{
    //Int Variables
    public int numOfGames;
    public int minNumOfPlayers;

    //Game and Modifier Lists
    public MinigameList gameList;
    public ModifierList modifierList;
    
}
