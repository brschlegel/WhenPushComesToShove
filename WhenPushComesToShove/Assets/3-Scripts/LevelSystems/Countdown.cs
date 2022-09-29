using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Countdown : MonoBehaviour
{
    public float countdownTime = 3;
    private float timer = 0;
    private TextMeshProUGUI text;
    public TextMeshProUGUI roomText;

    private Material mat;

    private void OnAwake()
    {
        mat = GetComponent<Material>();
    }

    private void OnEnable()
    {
        if (text != null)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);
            roomText.color = new Color(roomText.color.r, roomText.color.g, roomText.color.b, 1f);
            timer = countdownTime;
        }
        else
        {
            text = GetComponentInChildren<TextMeshProUGUI>();
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        text.text = Mathf.CeilToInt(timer).ToString();

        if (timer <= 0)
        {
            roomText.DOFade(0, .4f);
            text.DOFade(0, .4f).OnComplete(OnFadeComplete);
        }
    }

    private void OnFadeComplete()
    {
        gameObject.SetActive(false);
    }


}
