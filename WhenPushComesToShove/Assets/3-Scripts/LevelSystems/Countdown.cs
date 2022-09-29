using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Countdown : MonoBehaviour
{
    public float countdownTime = 3;
    private float timer = 0;
    private TextMeshProUGUI text;

    private void OnEnable()
    {
        if (text != null)
        {
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
            gameObject.SetActive(false);
        }
    }
}
