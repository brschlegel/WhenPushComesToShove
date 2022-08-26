using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class PlayerSetupMenuSpawn : MonoBehaviour
{
    public GameObject playerSetupMenuPrefab;
    public PlayerInput input;

    private void Awake()
    {
        //Spawns a menu for the player and tracks their index
        GameObject rootMenu = GameObject.Find("Main Layout");
        if (rootMenu != null)
        {
            GameObject setupMenu = Instantiate(playerSetupMenuPrefab, rootMenu.transform);
            input.uiInputModule = setupMenu.GetComponentInChildren<InputSystemUIInputModule>();
            setupMenu.GetComponent<PlayerSetupControls>().SetPlayerIndex(input.playerIndex);
        }
    }
}
