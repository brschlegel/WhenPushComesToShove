using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    [Header("BGM")]
    public FMOD.Studio.EventInstance bgm;

    [Header("Parameter Controls")]
    [Range(0f, 6f)] public float intensity = 1;
    public float deltaIntensity = 0;

    private void Awake()
    {
        if (instance != null)
            Debug.LogError("Found more than one audio manager in the scene");

        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        bgm = RuntimeManager.CreateInstance(FMODEvents.instance.bgm);
        bgm.start();
        bgm.release();
    }

    public void SetIntensity(float newIntensity)
    {
        intensity = newIntensity;
        if (intensity < 0) intensity = 0;
        if (intensity > 6) intensity = 6;

        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Intensity", intensity);
    }

    public void ChangeIntensity()
    {
        intensity += deltaIntensity;
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Intensity", intensity);
    }

    public void SetDeltaIntensity(float newDelta, float timer = 0f)
    {
        deltaIntensity = newDelta;
        if(timer != 0f)
            StartCoroutine("IntensityTimer", timer);
    }

    public IEnumerator IntensityTimer(float timer)
    {
        while(intensity < 6)
        {
            yield return new WaitForSeconds(timer);
            ChangeIntensity();
        }
    }

    public void StartMusic()
    {
        bgm.start();
    }

    public void StopMusic()
    {
        bgm.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    private void OnDestroy()
    {
        bgm.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
