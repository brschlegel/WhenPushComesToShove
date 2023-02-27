using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlaystationTests : MonoBehaviour
{

    PlayerInputManager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = GetComponent<PlayerInputManager>();
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlayerLeft(PlayerInput input)
    {
        Debug.Log(input.devices.Count);
       // manager.JoinPlayer(input.playerIndex, -1, input.currentControlScheme, InputSystem.devices[1]);
    }


}
