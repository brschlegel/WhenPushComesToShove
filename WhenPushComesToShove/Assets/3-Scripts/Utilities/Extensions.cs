using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public static class Extensions
{

    public static T GetRandomElement<T>(this List<T> list)
    {
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    //https://stackoverflow.com/questions/273313/randomize-a-listt
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0,n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    /// <summary>
    /// Clamps a given vector to a maximum magnitude
    /// </summary>
    /// <param name="v">Vector to be clamped</param>
    /// <param name="maxMagnitude">Maximum magnitude of returned vector</param>
    public static Vector2 Clamp(this Vector2 v, float maxMagnitude)
    {
        if (v.SqrMagnitude() > (maxMagnitude * maxMagnitude))
        {
            return v.normalized * maxMagnitude;
        }
        return v;
    }

    public static float GetCurrentClipLength(this Animator anim)
    {
        //Must update to ensure get current methods are accurate
        anim.Update(Time.deltaTime);
        return anim.GetCurrentAnimatorClipInfo(0)[0].clip.length / (anim.GetCurrentAnimatorStateInfo(0).speed * anim.speed);
    }

    //https://answers.unity.com/questions/530178/how-to-get-a-component-from-an-object-and-add-it-t.html
    /// <summary>
    /// Copys a component's values and pastes it into the new component through script
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="comp">New Component</param>
    /// <param name="other">Componetn to Copy</param>
    /// <returns></returns>
    public static T GetCopyOf<T>(this T comp, T other) where T : Component
    {
        Type type = comp.GetType();
        Type othersType = other.GetType();
        if (type != othersType)
        {
            Debug.LogError($"The type \"{type.AssemblyQualifiedName}\" of \"{comp}\" does not match the type \"{othersType.AssemblyQualifiedName}\" of \"{other}\"!");
            return null;
        }

        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default;
        PropertyInfo[] pinfos = type.GetProperties(flags);

        foreach (var pinfo in pinfos)
        {
            if (pinfo.CanWrite)
            {
                try
                {
                    pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
                }
                catch
                {
                    /*
                     * In case of NotImplementedException being thrown.
                     * For some reason specifying that exception didn't seem to catch it,
                     * so I didn't catch anything specific.
                     */
                }
            }
        }

        FieldInfo[] finfos = type.GetFields(flags);
        foreach (var finfo in finfos)
        {
            finfo.SetValue(comp, finfo.GetValue(other));
        }

        return comp as T;
    }
}

