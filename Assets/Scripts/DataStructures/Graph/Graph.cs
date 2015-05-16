using UnityEngine;
using System.Collections;

public class Graph {
    private NodeList nodes;

    public Graph() {
        this.nodes = new NodeList();
    }

    public virtual Node AddNode(string key, object data) {
        if (!nodes.ContainsKey(key)) {
            Node n = new Node(key, data);
            nodes.Add(n);
            return n;
        }
        return null;
    }

    public virtual void AddNode(Node n) {
        if (!nodes.ContainsKey(n.Key)) {
            nodes.Add(n);
        }
    }

    public virtual void AddDirectedEdge(string uKey, string vKey) {
        AddDirectedEdge(uKey, vKey, 0.0f);
    }

    public virtual void AddDirectedEdge(string uKey, string vKey, float cost) {
        if (nodes.ContainsKey(uKey) && nodes.ContainsKey(vKey)) {
            AddDirectedEdge(nodes[uKey], nodes[vKey], cost);
        }
    }

    public virtual void AddDirectedEdge(Node u, Node v) {
        AddDirectedEdge(u, v, 0.0f);
    }

    public virtual void AddDirectedEdge(Node u, Node v, float cost) {
        if (nodes.ContainsKey(u.Key) && nodes.ContainsKey(v.Key)) {
            u.AddDirected(v, cost);
        }
    }

    public virtual void AddUndirectedEdge(string uKey, string vKey) {
        AddUndirectedEdge(uKey, vKey, 0.0f);
    }

    public virtual void AddUndirectedEdge(string uKey, string vKey, float cost) {
        if (nodes.ContainsKey(uKey) && nodes.ContainsKey(vKey)) {
            AddUndirectedEdge(nodes[uKey], nodes[vKey], cost);
        }
    }

    public virtual void AddUndirectedEdge(Node u, Node v) {
        AddUndirectedEdge(u, v, 0);
    }

    public virtual void AddUndirectedEdge(Node u, Node v, float cost) {
        if (nodes.ContainsKey(u.Key) && nodes.ContainsKey(v.Key)) {
            u.AddDirected(v, cost);
            v.AddDirected(u, cost);
        }
    }

    public virtual bool Contains(Node n) {
        return Contains(n.Key);
    }

    public virtual bool Contains(string key) {
        return nodes.ContainsKey(key);
    }

    public virtual NodeList Nodes {
        get {
            return this.nodes;
        }
    }
}
