using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class HazianSetter : MonoBehaviour, ISerializationCallbackReceiver
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

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        if (updateVariables && !EditorApplication.isPlayingOrWillChangePlaymode)
        {
            SetVariables();

        }
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize() { }


#else
void ISerializationCallbackReceiver.OnBeforeSerialize() { }
void ISerializationCallbackReceiver.OnAfterDeserialize() { }
private void Start()
    {
        if(updateVariables)
        SetVariables();
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
