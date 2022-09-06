using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{

    public static CoroutineManager Instance { get; private set; }

    public static GameObject GetObject()
    {
        return Instance.gameObject;
    }

    public static Coroutine StartGlobalCoroutine(IEnumerator coroutine)
    {
        return Instance.StartCoroutine(coroutine);
    }
    public void Awake()
    {
        Init();
    }

    public void Init()
    {
        if (Instance != null)
        {
            Debug.Log("An instance of CoroutineManager already exists");
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
    }
}
