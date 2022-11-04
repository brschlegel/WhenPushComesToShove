using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    private GameObject rootObject;
    [SerializeField]
    private SpriteRenderer rootSprite;
    [SerializeField]
    private float explosionDuration;
    [SerializeField]
    private Hitbox explosionHitbox;
    [SerializeField]
    private GameObject vfxPrefab;

    // Start is called before the first frame update
    void Start()
    {
        explosionHitbox.owner = rootObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            Explode();
        }
    }

    public void Explode()
    {
        Instantiate(vfxPrefab, transform.position, Quaternion.identity);
        rootSprite.enabled = false;
        explosionHitbox.gameObject.SetActive(true);
        CoroutineManager.StartGlobalCoroutine(WaitToDestroy());
    }

    private IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(explosionDuration);
        Destroy(rootObject);
    }
}
