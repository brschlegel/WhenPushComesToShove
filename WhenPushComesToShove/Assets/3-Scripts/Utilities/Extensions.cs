using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public static class Extensions
{
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

