using UnityEngine;
using System.Collections;

public class GraphController : MonoBehaviour {
    public static GraphController instance;

    [SerializeField]
    private Color defaultNodeColor;

    private Graph graph;

    public Graph Graph {
        get {
            return graph;
        }
    }

    public void AddEdge(Node from, Node to, float cost) {
        graph.AddNode(from);
        graph.AddNode(to);

        graph.AddDirectedEdge(from, to, cost);
    }

    public Node GetNode(string key) {
        return graph.Nodes[key];
    }

    public void ResetColor() {
        foreach (Node n in graph.Nodes) {
            ((GameObject)n.Data).renderer.material.color = defaultNodeColor;            
        }
    }

    protected void Awake() {
        instance = this;

        graph = new Graph();
    }

    protected void OnDestroy() {
        if (instance != null) {
            instance = null;
        }
    }

    /// <summary>
    /// Draw the edges of the graph for debugging purposes.
    /// </summary>
    protected void OnDrawGizmos() {
        if (graph == null) return;
        foreach (Node n in graph.Nodes) {
            Vector3 start = ((GameObject)n.Data).transform.position;
            foreach (EdgeToNeighbour b in n.Neighbours) {
                Gizmos.DrawLine(start, ((GameObject)b.Neighbour.Data).transform.position);
            }
        }
    }
}
