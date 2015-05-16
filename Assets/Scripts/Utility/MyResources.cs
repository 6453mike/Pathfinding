using UnityEngine;
using System.Collections.Generic;

public static class MyResources {
    private static Dictionary<string, GameObject> inMemory = new Dictionary<string, GameObject>();
    
    public static GameObject Load(string s) {
        GameObject o = null;
        if (inMemory.ContainsKey(s)) {
            inMemory.TryGetValue(s, out o);
        } else {
            o = Resources.Load(s) as GameObject;
            inMemory.Add(s, o);
        }
        return o;
    }
}
