using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpikeState {Off, Threatening, On}
public class Spikes : MonoBehaviour
{
    public bool startOn = false;
    [HideInInspector]
    public SpikeState state;
  
    private Animator anim;
    private Hitbox hitbox;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        hitbox = GetComponentInChildren<Hitbox>();
        hitbox.gameObject.SetActive(startOn);
        if(startOn)
        {
            Activate();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            switch(state)
            {
                case SpikeState.Off:
                Threaten();
                break;
                case SpikeState.Threatening:
                Activate();
                break;
                case SpikeState.On:
                Deactivate();
                break;

            }
        }
    }

    public void Threaten()
    {
        anim.Play("Spikes_Threaten");
        state = SpikeState.Threatening;
    }

    public void Activate()
    {
        anim.Play("Spikes_Activate");
        hitbox.gameObject.SetActive(true);
        state = SpikeState.On;
    }

    public void Deactivate()
    {
        anim.Play("Spikes_Deactivate");
        hitbox.gameObject.SetActive(false);
        state = SpikeState.Off;
    }
}
