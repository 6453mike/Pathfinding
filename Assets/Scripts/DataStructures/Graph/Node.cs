using UnityEngine;
using System.Collections;

public class Node {
    private string key;
    private object data;
    private AdjacencyList neighbours;

    public Node(string key, object data) {
        this.key = key;
        this.data = data;
        this.neighbours = new AdjacencyList();
    }

    public virtual string Key {
        get {
            return key;
        }
        private set {
            key = value;
        }
    }

    public virtual object Data {
        get {
            return data;
        }
        set {
            data = value;
        }
    }

    public virtual AdjacencyList Neighbours {
        get {
            return neighbours;
        }
        private set {
            neighbours = value;
        }
    }

    protected internal virtual void AddDirected(Node n) {
        AddDirected(new EdgeToNeighbour(n));
    }

    protected internal virtual void AddDirected(Node n, float cost) {
        AddDirected(new EdgeToNeighbour(n, cost));
    }

    protected internal virtual void AddDirected(EdgeToNeighbour e) {
        neighbours.Add(e);
    }
}
