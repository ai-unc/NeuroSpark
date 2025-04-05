using System;       // <-- Important for [Serializable]
using UnityEngine;

public static class JSONHelper
{
    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }

    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array, bool prettyPrint = false)
    {
        Wrapper<T> wrapper = new Wrapper<T>
        {
            Items = array
        };
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }
}
