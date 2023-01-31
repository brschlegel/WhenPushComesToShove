using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveBetweenPoints : MonoBehaviour
{
    [SerializeField]
    private Vector2 firstPoint;
    [SerializeField]
    private Vector2 secondPoint;

    Vector3 initialPos;

    [SerializeField]
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void Init()
    {
        initialPos = transform.position;
        Sequence seq = DOTween.Sequence();

        seq.Join(transform.DOMove(initialPos + (Vector3)firstPoint, speed)).SetSpeedBased().Join(transform.DOMove(initialPos + (Vector3)secondPoint, speed)).SetSpeedBased().SetLoops(-1, LoopType.Restart);
    }
    // Update is called once per frame
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(gameObject.transform.position + (Vector3)firstPoint, 0.3f);
        Gizmos.DrawSphere(gameObject.transform.position + (Vector3)secondPoint, 0.3f);
        Gizmos.DrawLine(gameObject.transform.position + (Vector3)firstPoint, gameObject.transform.position + (Vector3)secondPoint);
    }
}
