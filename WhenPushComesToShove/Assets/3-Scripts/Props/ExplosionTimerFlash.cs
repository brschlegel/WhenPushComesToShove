using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Explosion))]
public class ExplosionTimerFlash : MonoBehaviour
{
    public float maxTime = 5;
    [SerializeField]
    private SpriteRenderer render;
    [SerializeField]
    private AnimationCurve buildUpCurve;
    [SerializeField]
    private float transitionToExplosionOffset = 0.5f;
    [SerializeField]
    private AnimationCurve explodeCurve;

    private Explosion explosion;
    private float timer = 0;
    private bool hasExploded;
    // Start is called before the first frame update
    void Start()
    {
        hasExploded = false;
        explosion = GetComponent<Explosion>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasExploded)
        {
            timer += Time.deltaTime;
            //Two seperate curves: One to slowly ramp up, and one to quickly blink into explosion
            if (timer <= maxTime - transitionToExplosionOffset)
            {
                render.material.SetFloat("_FlashTime", buildUpCurve.Evaluate(timer / maxTime - transitionToExplosionOffset));
            }
            else
            {
                timer += Time.deltaTime;
                render.material.SetFloat("_FlashTime", explodeCurve.Evaluate((maxTime - timer) / transitionToExplosionOffset));
                if (timer >= maxTime)
                {
                    explosion.Explode();
                    hasExploded = true;
                }

            }
        }
    }
}
