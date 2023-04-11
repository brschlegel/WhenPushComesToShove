using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using DG.Tweening;

public class Countdown : MonoBehaviour
{
    public float countdownTime = 3;
    private float timer = 0;
    private TextMeshProUGUI text;

    public UnityAction onCountdownEnded;

    private Material mat;

    private int prevTime;
    private FMOD.Studio.EventInstance countdownInstance;

    private void OnAwake()
    {
        mat = GetComponent<Material>();
    }

    private void OnEnable()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);
        timer = countdownTime;
        prevTime = (int)timer;
        countdownInstance = AudioManager.instance.PlayWithInstance(FMODEvents.instance.countdown);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        text.text = Mathf.CeilToInt(timer).ToString();
        if (Mathf.CeilToInt(timer) < prevTime)
        {
            countdownInstance.keyOff();
            prevTime = Mathf.CeilToInt(timer);
        }
            
        if (timer <= 0)
        {
            text.DOFade(0, .4f).OnComplete(OnFadeComplete);
        }
    }

    private void OnFadeComplete()
    {
        gameObject.SetActive(false);
        onCountdownEnded?.Invoke();
    }


}
