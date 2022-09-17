using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushDoorEndCondition : BaseEndCondition
{
    int numOfPlayersInTrigger = 0;

    protected override void TestCondition()
    {
        //Ensures the room doesn't transition if there isn't enough players
        if (PlayerConfigManager.Instance.GetPlayerConfigs().Count < PlayerConfigManager.Instance.GetMinPlayer())
            return;

        //Tests if the number of players in the trigger box matches the number of players in the game
        if(numOfPlayersInTrigger == PlayerConfigManager.Instance.GetPlayerConfigs().Count)
        {
            PlayerConfigManager.Instance.levelInitRef.lockPlayerSpawn = true;
            base.TestCondition();
        }
            
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            numOfPlayersInTrigger++;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            numOfPlayersInTrigger--;
    }
}
