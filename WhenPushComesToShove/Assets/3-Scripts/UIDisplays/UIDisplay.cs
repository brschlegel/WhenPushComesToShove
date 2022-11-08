using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIDisplay : MonoBehaviour
{
    public bool isDone;

    public abstract void ShowDisplay();
    public abstract void HideDisplay();

}
