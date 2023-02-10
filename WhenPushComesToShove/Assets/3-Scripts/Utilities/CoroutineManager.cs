using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Coroutines will stop execution if the monobehaviour it was started on is destroyed OR disabled
//The CoroutineManager singleton allows us to create global coroutines from any monobehaviour
public class CoroutineManager : MonoBehaviour
{

    public static CoroutineManager Instance { get; private set; }

    public static GameObject GetObject()
    {
        return Instance.gameObject;
    }

    public static IEnumerator StartGlobalCoroutine(IEnumerator enumerator)
    {
        Instance.StartCoroutine(enumerator);
        return enumerator;
    }

    public static void StopGlobalCoroutine(IEnumerator enumerator)
    {
        Instance.StopCoroutine(enumerator);
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
