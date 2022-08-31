using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script to check for players in the doors trigger.
public class DoorScript : MonoBehaviour
{

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            int playerIndex = collision.gameObject.GetComponentInChildren<PlayerInputHandler>().playerConfig.PlayerIndex;
            PlayerConfigManager.Instance.ReadyPlayer(playerIndex);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            int playerIndex = collision.gameObject.GetComponentInChildren<PlayerInputHandler>().playerConfig.PlayerIndex;
            PlayerConfigManager.Instance.UnreadyPlayer(playerIndex);
        }
    }

}
