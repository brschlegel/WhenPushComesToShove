using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazianSetter : MonoBehaviour
{
    public GameVariables gameVariables;
    [SerializeField] bool updateVariables = true;
    [Space]
    [Space]

    [SerializeField] PathGenerator pathGen;
    [SerializeField] ModifierManager modManager;
    [SerializeField] PlayerConfigManager playerManager;

    
#if UNITY_EDITOR
    //Will update variables when scene is reloaded or variables change
    private void OnValidate()
    {
        if (updateVariables)
        {
            SetVariables();

        }
    }
#else
private void Start()
    {
        if (updateVariables)
        {
            SetVariables();
        }
    }
#endif


    private void SetVariables()
    {
        Debug.Log("Resetting Variables");
        //Path Generator variables
        pathGen.path.Clear();
        pathGen.numOfGames = gameVariables.numOfGames;
        pathGen.minigamePool = gameVariables.gameList;

        //Modifier Manager variables
        modManager.modifierPoolSO = gameVariables.modifierList;
        modManager.debugList = null;

        //Player Config variables
        playerManager.minPlayers = gameVariables.minNumOfPlayers;
    }
}
