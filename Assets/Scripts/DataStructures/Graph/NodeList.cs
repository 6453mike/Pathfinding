using UnityEngine;
using System.Collections;

public class NodeList : IEnumerable {
    private Hashtable data = new Hashtable();

    public int Count {
        get {
            return data.Count;
        }
    }

    public virtual void Add(Node n) {
        data.Add(n.Key, n);
    }

    public virtual void Remove(Node n) {
        data.Remove(n.Key);
    }

    public virtual bool ContainsKey(string key) {
        return data.ContainsKey(key);
    }

    public virtual void Clear() {
        data.Clear();
    }

    public virtual Node this[string key] {
        get {
            return (Node)data[key];
        }
    }

    public IEnumerator GetEnumerator() {
        foreach (DictionaryEntry o in data) {
            if (o.Value == null) {
                break;
            }

            yield return o.Value;
        }
    }
}
