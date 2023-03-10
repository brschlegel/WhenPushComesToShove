using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Explosion))]
public class ExplosionTimerFlash : MonoBehaviour
{
   
    [SerializeField]
    private SpriteRenderer render;
    [SerializeField]
    private AnimationCurve buildUpCurve;
    [SerializeField]
    private float transitionToExplosionOffset = 0.5f;
    [SerializeField]
    private AnimationCurve explodeCurve;
    [SerializeField]
    private PlayAnimOnce startExplosionAnim;

    private Explosion explosion;
    private float timer = 0;
    private bool hasExploded;
    private bool animStarted;
    private float maxTime = 5;

    public ParticleSystem smallExplosionRing;
    public ParticleSystem mediumExplosionRing;
    public ParticleSystem largeExplosionRing;

    public float MaxTime
    {
        get { return maxTime;}
        set
        {
            maxTime = value;
            startExplosionAnim.Speed = 1 / (maxTime - transitionToExplosionOffset);
            smallExplosionRing.startLifetime = maxTime;
            mediumExplosionRing.startLifetime = maxTime;
            largeExplosionRing.startLifetime = maxTime;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        hasExploded = false;
        startExplosionAnim.Speed = 1 / (maxTime - transitionToExplosionOffset);
        startExplosionAnim.PlayAnim();
        explosion = GetComponent<Explosion>();
        //smallExplosionRing.gameObject.SetActive(true);
        //mediumExplosionRing.gameObject.SetActive(true);
        largeExplosionRing.gameObject.SetActive(true);
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
