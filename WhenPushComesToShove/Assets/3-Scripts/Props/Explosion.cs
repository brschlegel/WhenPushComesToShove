using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void emptyDelegate();
public class Explosion : MonoBehaviour
{
    [SerializeField]
    private GameObject rootObject;
    [SerializeField]
    private SpriteRenderer rootSprite;
    [SerializeField]
    private float explosionDuration;
    private List<Hitbox> explosionHitboxes;
    [SerializeField]
    private GameObject vfxPrefab;

    public event emptyDelegate onExplode;

    // Start is called before the first frame update
    void Start()
    {
        explosionHitboxes = new List<Hitbox>(GetComponentsInChildren<Hitbox>(true));
        
        foreach(Hitbox h in explosionHitboxes)
        {
            h.owner = rootObject;
        }
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
        foreach(Hitbox h in explosionHitboxes)
        {
            h.gameObject.SetActive(true);
        }
        CoroutineManager.StartGlobalCoroutine(WaitToDestroy());
        rootObject.GetComponentInChildren<Rigidbody2D>().velocity = Vector2.zero;
        onExplode?.Invoke();
    }

    private IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(explosionDuration);
        Destroy(rootObject);
    }
}
