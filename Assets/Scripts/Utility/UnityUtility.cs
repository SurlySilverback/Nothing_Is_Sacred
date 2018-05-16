﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnityUtility
{
    public static T GetSafeComponent<T>(GameObject gameObject)
    {
        T component = gameObject.GetComponent<T>();
        if (component == null)
        {
            Debug.LogError("Component requested is null!");
        }
        return component;
    }

    public static void Swap<T>(ref T lhs, ref T rhs)
    {
        T temp;
        temp = lhs;
        lhs = rhs;
        rhs = temp;
    }
}