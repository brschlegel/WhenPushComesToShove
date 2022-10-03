using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public delegate void DashEvent (Vector3 v);
public class PlayerDashScript : MonoBehaviour
{
    [HideInInspector] public ProjectileMode pMode;
    [SerializeField] private float dashSpeed = 15;
    [SerializeField] private float dashTime = 1;

    public event DashEvent onDashStart;

    private PlayerMovementScript mover;
    private PlayerInputHandler handler;

    [SerializeField] private PlayerCollisions collider;

    public void Start()
    {
        mover = GetComponent<PlayerMovementScript>();
        handler = GetComponent<PlayerInputHandler>();
    }

    public void OnDash(CallbackContext context)
    {
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
        collider.tagsToIgnoreCollision.Add("Player");
        collider.tagsToIgnoreCollision.Add("Enemy");

        yield return new WaitForSeconds(dashTime);

        if (collider.tagsToIgnoreCollision.Contains("Player"))
        {
            collider.tagsToIgnoreCollision.Remove("Player");
        }

        if (collider.tagsToIgnoreCollision.Contains("Enemy"))
        {
            collider.tagsToIgnoreCollision.Remove("Enemy");
        }
    }
}
