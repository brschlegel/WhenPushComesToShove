using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : Health
{
    public PlayerInputHandler playerRef;
    public UnityEvent onDeath;
    
    public override void Die()
    {
        playerRef.sr.color = new Color(playerRef.sr.color.r, playerRef.sr.color.g, playerRef.sr.color.b, .5f);

        dead = true;
        playerRef.playerConfig.IsDead = true;

        if (PlayerConfigManager.Instance.CheckAllPlayerDeath())
        {
            //GameObject.Find("Path").GetComponent<LevelManager>().ResetPath();
        }
        onDeath?.Invoke();
    }

    public void ResetHealth()
    {
        dead = false;
        currentHealth = maxHealth;
        playerRef.sr.color = new Color(playerRef.sr.color.r, playerRef.sr.color.g, playerRef.sr.color.b, 1f);
    }

    private void Update()
    {
        if (dead)
        {
            currentHealth = 0;
        }
    }
}
