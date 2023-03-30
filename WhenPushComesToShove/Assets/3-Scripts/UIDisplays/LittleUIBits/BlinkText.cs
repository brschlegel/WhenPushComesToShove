using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class BlinkText : MonoBehaviour
{
    [SerializeField]
    private float tweenDuration;
    [SerializeField]
    private Ease ease;
    
    private TextMeshProUGUI text;
    private Tween fadeTween;
    private void OnEnable()
    {
        text = GetComponent<TextMeshProUGUI>();
        fadeTween = text.DOFade(0, tweenDuration).SetEase(ease).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDisable()
    {
        fadeTween.Kill();
    }


    
}
