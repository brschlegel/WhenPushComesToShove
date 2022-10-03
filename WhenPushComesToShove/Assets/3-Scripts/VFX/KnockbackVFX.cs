using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackVFX : MonoBehaviour
{
    [SerializeField]
    private Transform exclamationTransform;
    [SerializeField]
    private ParticleSystem exclamationPS;
    [SerializeField]
    private Transform gustTransform;
    [SerializeField]
    private ParticleSystem gustPS;
    [SerializeField]
    private Material windGustMat;
    [SerializeField]
    private Texture horizontalSheet;
    [SerializeField]
    private Texture verticalSheet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play(float force, Vector2 direction)
    {
        exclamationTransform.right = direction;
        exclamationPS.Play();

        gustTransform.right = direction;

        
        float xSign = Mathf.Sign(direction.x);
        float ySign = Mathf.Sign(direction.y);
        Vector2 compareTo = -xSign * Vector2.right;
        float angle = Vector2.Angle(compareTo, -direction);

        if(angle <= 45)
        {
            
            gustPS.transform.localScale = new Vector3(Mathf.Abs(gustPS.transform.localScale.x) * xSign,gustPS.transform.localScale.y, gustPS.transform.localScale.z ); 
            gustTransform.right = Vector2.right;
            windGustMat.mainTexture = horizontalSheet;
        }
        else 
        {
            gustPS.transform.localScale = new Vector3(gustPS.transform.localScale.x, Mathf.Abs(gustPS.transform.localScale.y) * ySign, gustPS.transform.localScale.z );
            gustTransform.right = Vector2.right;
            windGustMat.mainTexture = verticalSheet;
        }

        // if(angle >= 90)
        // {
        //     gustTransform.right = direction;
        // }
        // else
        // {
        //     gustTransform.right = Vector2.right;
        // }
        gustPS.Play();
    }
}
