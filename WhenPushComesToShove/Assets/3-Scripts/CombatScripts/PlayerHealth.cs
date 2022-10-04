using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : Health
{
    public PlayerInputHandler playerInputRef;
    [SerializeField] private PlayerCollisions collider;

    public UnityEvent onDeath;
 
    private void OnEnable()
    {
        LevelManager.onNewRoom += ResetHealth;
        LevelManager.onEndGame += ResetHealth;
    }

    private void OnDisable()
    {
        LevelManager.onNewRoom -= ResetHealth;
        LevelManager.onEndGame -= ResetHealth;
    }

    public override void Die()
    {
        playerInputRef.sr.color = new Color(playerInputRef.sr.color.r, playerInputRef.sr.color.g, playerInputRef.sr.color.b, .3f);

        dead = true;
        playerInputRef.playerConfig.IsDead = true;

        //Unassign Ghost Shove Tags
        KnockbackHitHandler kbReciever = transform.parent.GetComponentInChildren<KnockbackHitHandler>();
        if (kbReciever.tagsToIgnore.Contains("GhostShove"))
        {
            kbReciever.tagsToIgnore.Remove("GhostShove");
            kbReciever.tagsToIgnore.Add("Shove");
        }

        KnockbackAlongAxis kbHit = transform.parent.GetComponentInChildren<KnockbackAlongAxis>();
        kbHit.gameObject.tag = "GhostShove";

        //Allow passage through players and enemies
        collider.tagsToIgnoreCollision.Add("Player");
        collider.tagsToIgnoreCollision.Add("Enemy");

        onDeath?.Invoke();
    }

    public void ResetHealth()
    {
        //Prevent passage through players and enemies
        if (dead)
        {
            collider.tagsToIgnoreCollision.Remove("Player");
            collider.tagsToIgnoreCollision.Remove("Enemy");
        }

        dead = false;
        currentHealth = maxHealth;
        playerInputRef.sr.color = new Color(playerInputRef.sr.color.r, playerInputRef.sr.color.g, playerInputRef.sr.color.b, 1f);

        //Reassign Ghost Shove Tags
        KnockbackHitHandler kbReciever = transform.parent.GetComponentInChildren<KnockbackHitHandler>();
        if (!kbReciever.tagsToIgnore.Contains("GhostShove"))
        {
            kbReciever.tagsToIgnore.Add("GhostShove");
            kbReciever.tagsToIgnore.Remove("Shove");
        }

        KnockbackAlongAxis kbHit = transform.parent.GetComponentInChildren<KnockbackAlongAxis>();
        kbHit.gameObject.tag = "Shove";
    }

    private void Update()
    {
        if (dead)
        {
            currentHealth = 0;
        }
    }
}
