using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Code from: https://www.youtube.com/watch?v=AuZNU7JTeWQ
public class ScrollingText : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    public float scrollSpeed = 5;
    private TextMeshProUGUI cloneTMP;

    private RectTransform textTransform;
    private string text;
    private string tempText;

    void OnEnable()
    {
        textTransform = tmp.GetComponent<RectTransform>();

        cloneTMP = Instantiate(tmp) as TextMeshProUGUI;
        RectTransform cloneTransform = cloneTMP.GetComponent<RectTransform>();
        cloneTransform.SetParent(tmp.transform);
        cloneTransform.anchoredPosition = new Vector3(0, 0f, 0);
        cloneTransform.anchorMin = new Vector2(1, .5f);
        cloneTransform.localScale = Vector3.one;

        StartCoroutine("Scroll");
    }

    IEnumerator Scroll()
    {
        Debug.Log("Scrolling");
        float width = tmp.preferredWidth;//tmp.preferredWidth * tmp.fontSize / 8 / 8.4375f;
        Vector3 startPosition = textTransform.position;

        float scrollPosition = 0;

        while(true)
        {
            //Debug.Log("Scroll Position: " + scrollPosition);
            Debug.Log("Width: " + width);
            //Debug.Log("Scroll Position % Width = " + scrollPosition % width);
            textTransform.anchoredPosition = new Vector3(-scrollPosition % width, textTransform.anchoredPosition.y, 0);
            scrollPosition += scrollSpeed * 20 * Time.deltaTime;
            Debug.Log("Position X: " + textTransform.position.x);
            if (textTransform.anchoredPosition.x <= -width)
            {
                Debug.Log("Reset");
                scrollPosition = 0;
            }
                

            yield return null;
        }
    }    
}
