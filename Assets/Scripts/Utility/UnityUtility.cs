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

    public static void DestroyChildren(Transform parent)
    {
        foreach(Transform transform in parent)
        {
            GameObject.Destroy(transform.gameObject);
        }
    }
    
    public static string GetGameObjectPath(GameObject obj)
    {
        string path = "/" + obj.name;
        while (obj.transform.parent != null)
        {
            obj = obj.transform.parent.gameObject;
            path = "/" + obj.name + path;
        }
        return path;
    }
}