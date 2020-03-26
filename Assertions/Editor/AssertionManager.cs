using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[InitializeOnLoadAttribute]
public static class AssertionManager
{
    // register an event handler when the class is initialized
    static AssertionManager()
    {
        EditorApplication.playModeStateChanged += LogPlayModeState;
    }

    private static void LogPlayModeState(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            CheckAll();
        }
    }

    private static void CheckAll()
    {

        var allObjects = GameObject.FindObjectsOfType<Transform>();

        int errors = 0;
        foreach (var t in allObjects)
            if (!Check(t))
                errors++;

        if (errors > 0)
            EditorApplication.ExitPlaymode();

    }

    private static bool Check(Transform t)
    {
        bool noErrorsFound = true;

        var components = t.GetComponents<MonoBehaviour>();

        foreach (var c in components)
        {
            var fields = c.GetType().GetFields(
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Instance);

            foreach (var f in fields)
            {
                var notnull = Attribute.GetCustomAttribute(f, typeof(AssertNotNullAttribute)) as AssertNotNullAttribute;
                if (notnull != null && f.GetValue(c) == null)
                {
                    Debug.LogError($"Assert failed: {c.GetType().Name}::{f.Name}", c);
                    noErrorsFound = false;
                }
            } // fields
        } // components

        return noErrorsFound;
    }

}
