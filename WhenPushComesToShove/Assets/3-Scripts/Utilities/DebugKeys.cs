using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugKeys : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.D))
        {
            foreach(Transform t in GameState.players)
            {
                PlayerHealth h = t.GetComponentInChildren<PlayerHealth>();
                if(!h.dead)
                {
                    h.TakeDamage(9999, "Debug");
                    return;
                }
                

            }
        }
    }
}
