using UnityEngine;
using System.Collections;

public class Dijkstra : Heuristic {
    public override float CalculateHeuristic(Node currentNode, Node goal, Graph graph) {
        // This is the null heuristic. So just return 0.
        return 0.0f;
    }

    protected void Update() { }
}
