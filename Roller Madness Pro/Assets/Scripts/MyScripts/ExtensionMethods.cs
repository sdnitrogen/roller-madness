using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public static class ExtensionMethods
{
    public static T FindObjectOfType <T> (this UnityEngine.Object unityObject) where T : UnityEngine.Object
    {
        return UnityEngine.Object.FindObjectOfType(typeof(T)) as T;
    }

    public static T[] FindObjectsOfType <T> (this UnityEngine.Object unityObject) where T : UnityEngine.Object
    {
        return UnityEngine.Object.FindObjectsOfType(typeof(T)) as T[];
    }
}