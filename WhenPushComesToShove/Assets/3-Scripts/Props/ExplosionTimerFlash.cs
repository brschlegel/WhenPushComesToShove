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
            if (timer <= maxTime - transitionToExplosionOffset)
            {
                render.material.SetFloat("_FlashTime", buildUpCurve.Evaluate(timer / maxTime - transitionToExplosionOffset));
            }
            //Sean: this is what i was talking about, the transitionToExplosion offset is always a fixed time. 
            //You could have the animation start here, the first time this else is called. Message me if you have any q's about it
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
