using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ModifierSelectedUIDisplay : UIDisplay
{
    [HideInInspector]
    public BaseModifier modifier;
    [SerializeField]
    private Image modifierImage;
    [SerializeField]
    private TextMeshProUGUI modifierName;
    [SerializeField]
    private TextMeshProUGUI modifierDescription;
    [SerializeField]
    private GameObject modifierSelectedText;

    [SerializeField]
    private float delay;

    public override void ShowDisplay()
    {
        modifierImage.sprite = modifier.icon;
        modifierName.text = modifier.name;
        modifierDescription.text = modifier.description;
        modifierImage.gameObject.SetActive(true);
        modifierName.gameObject.SetActive(true);
        modifierDescription.gameObject.SetActive(true);
        modifierSelectedText.SetActive(true);
        isDone = false;
        CoroutineManager.StartGlobalCoroutine(WaitToFinish());
    }

    public override void HideDisplay()
    {
        modifierImage.gameObject.SetActive(false);
        modifierName.gameObject.SetActive(false);
        modifierDescription.gameObject.SetActive(false);
        modifierSelectedText.SetActive(false);
    }

    private IEnumerator WaitToFinish()
    {
        yield return new WaitForSeconds(delay);
        isDone = true;
        HideDisplay();
    }
}
