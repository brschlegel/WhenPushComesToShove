using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private void Awake()
    {
        if (instance != null)
            Debug.LogError("Found more than one audio manager in the scene");

        instance = this;
    }

    public void PlayOneShot(EventReference sound)
    {
        RuntimeManager.PlayOneShot(sound);
    }

    public FMOD.Studio.EventInstance PlayWithInstance(EventReference sound)
    {
        FMOD.Studio.EventInstance instance = RuntimeManager.CreateInstance(sound);

        instance.start();
        instance.release();

        return instance;
    }

    //Credit: https://alessandrofama.com/tutorials/fmod/unity/playoneshot-parameters
    public FMOD.Studio.EventInstance PlayOneShot(EventReference sound, params(string name, float value)[] parameters)
    {
        FMOD.Studio.EventInstance instance = RuntimeManager.CreateInstance(sound);

        foreach(var (name, value) in parameters)
        {
            instance.setParameterByName(name, value);
        }

        Debug.Log("Set parameters");
        instance.start();
        instance.release();

        return instance;
    }
}
