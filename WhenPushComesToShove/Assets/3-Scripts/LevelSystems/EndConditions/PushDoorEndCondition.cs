using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushDoorEndCondition : BaseEndCondition
{
    int numOfPlayersInTrigger = 0;
    List<PlayerConfiguration> playerConfigs;
    List<GameObject> lightsToTurnOff = new List<GameObject>();
    List<GameObject> playersInTrigger = new List<GameObject>();

    //dont need to init
    public override void Init() {}
    public override bool TestCondition()
    {

        //Ensure that players can join in the lobby
        PlayerConfigManager.Instance.levelInitRef.UnlockPlayerSpawn();

        SetActiveLights();

        //Ensures the room doesn't transition if there isn't enough players
        if (PlayerConfigManager.Instance.GetPlayerConfigs().Count < PlayerConfigManager.Instance.GetMinPlayer())
            return false;

        //Tests if the number of players in the trigger box matches the number of players in the game
        if(numOfPlayersInTrigger == PlayerConfigManager.Instance.GetPlayerConfigs().Count)
        {
            PlayerConfigManager.Instance.levelInitRef.lockPlayerSpawn = true;
            this.GetComponent<Animator>().SetTrigger("Opened");
            Debug.Log("Opened!");
            return true;
        }

        return false;
            
    }

    /// <summary>
    /// Updates the lights based on which players are at the door
    /// </summary>
    private void SetActiveLights()
    {
        playerConfigs = PlayerConfigManager.Instance.GetPlayerConfigs();
        Transform lightParent = transform.GetChild(0);
        //Populates the list with the lights
        for(int i = 0; i < lightParent.childCount; i++)
        {
            lightsToTurnOff.Add(lightParent.GetChild(i).gameObject);
        }

        //Removes an turned on lights from the list
        foreach(PlayerConfiguration config in playerConfigs)
        {
            //Checks if it's a player by the door
            foreach(GameObject player in playersInTrigger)
            {
                if(config.PlayerObject == player)
                {
                    lightParent.GetChild(config.PlayerIndex).gameObject.SetActive(true);
                    lightsToTurnOff.Remove(lightParent.GetChild(config.PlayerIndex).gameObject);
                }
            }          
        }

        //turns off any lights still in the list
        foreach(GameObject light in lightsToTurnOff)
        {
            light.SetActive(false);
        }

        //Clears list
        lightsToTurnOff.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            this.GetComponent<Animator>().SetTrigger("Locked");
            Debug.Log("Locked!");
            numOfPlayersInTrigger++;
            playersInTrigger.Add(collision.gameObject);
        }
            
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            numOfPlayersInTrigger--;
            playersInTrigger.Remove(collision.gameObject);
        }           
    }
}
