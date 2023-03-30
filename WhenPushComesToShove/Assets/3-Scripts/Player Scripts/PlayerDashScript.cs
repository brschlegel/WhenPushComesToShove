using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public delegate void DashEvent (Vector3 v);
public class PlayerDashScript : MonoBehaviour
{
    public event DashEvent onDashStart;

    [HideInInspector] public ProjectileMode pMode;

    public float dashSpeed = 10000;
    [SerializeField] private float dashTime = 1;
    [SerializeField] private PlayerCollisions collider;
    [SerializeField] private HitHandler hurtboxHandler;
    [SerializeField] private List<string> tagsToIgnoreDuringDash;
    [SerializeField] private ParticleSystem dashVFX;

    private PlayerMovementScript mover;
    private PlayerInputHandler handler;

    public void Start()
    {
        mover = GetComponent<PlayerMovementScript>();
        handler = GetComponent<PlayerInputHandler>();
        LevelManager.onNewRoom += pMode.StopForce;
        LevelManager.onModifierRoom += pMode.StopForce;
    }

    public void OnDash(CallbackContext context)
    {
        LoggingInfo.instance.dashUses[handler.playerConfig.PlayerIndex] += 1;
        handler.LockAction(dashTime, null);
        PerformDash(mover);
    }

    /// <summary>
    /// A function called in input handler to trigger a dash
    /// </summary>
    /// <param name="playerIndex"></param>
    public void PerformDash(PlayerMovementScript mover)
    {
        Vector2 dashDirection = DetermineDashDirection(mover);
        if (dashDirection.x > 0)
            dashVFX.transform.localScale = new Vector3(1, 1, 1);
        else if (dashDirection.x < 0)
            dashVFX.transform.localScale = new Vector3(-1, 1, 1);
        dashVFX.Play();
        AudioManager.instance.PlayOneShot(FMODEvents.instance.dash);
        if (pMode != null)
        {
            pMode.AddForce(dashDirection * dashSpeed);
            StartCoroutine(ColliderCooldown());
            onDashStart?.Invoke(dashDirection);
        }
    }

    /// <summary>
    /// Helper function to determine if the direction of a dash
    /// </summary>
    /// <returns></returns>
    public Vector2 DetermineDashDirection(PlayerMovementScript mover)
    {
        Vector2 direction = Vector2.zero;

        if (mover.GetMoveDirection() != Vector3.zero)
        {
            direction = mover.GetMoveDirection();
        }
        else if (mover.GetAimDirection() != Vector2.zero)
        {
            direction = mover.GetAimDirection();
        }
        else
        {
            direction = mover.player.right;
        }

        return direction;
    }

    private IEnumerator ColliderCooldown()
    {
        foreach (string tag in tagsToIgnoreDuringDash)
        {
            collider.tagsToIgnoreCollision.Add(tag);
            hurtboxHandler.tagsToIgnore.Add(tag);
        }

        //collider.tagsToIgnoreCollision.Add("Player");
        //collider.tagsToIgnoreCollision.Add("Enemy");
        //collider.tagsToIgnoreCollision.Add("Hazard");

        yield return new WaitForSeconds(dashTime);

        foreach (string tag in tagsToIgnoreDuringDash)
        {
            if (collider.tagsToIgnoreCollision.Contains(tag))
            {
                collider.tagsToIgnoreCollision.Remove(tag);
            }

            if (hurtboxHandler.tagsToIgnore.Contains(tag))
            {
                hurtboxHandler.tagsToIgnore.Remove(tag);
            }
        }
    }
}
