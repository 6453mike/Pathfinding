using UnityEngine;
using System.Collections;

public class AdjacencyList : CollectionBase {
    protected internal virtual void Add(EdgeToNeighbour e) {
        base.InnerList.Add(e);
    }

    public virtual EdgeToNeighbour this[int index] {
        get {
            return (EdgeToNeighbour)base.InnerList[index];
        }
        set {
            base.InnerList[index] = value;
        }
    }
}
