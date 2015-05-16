using UnityEngine;
using System.Collections;

public class Euclidean : Heuristic {
    public override float CalculateHeuristic(Node currentNode, Node goal, Graph graph) {
        Vector3 pos1 = ((GameObject)currentNode.Data).transform.position;
        Vector3 pos2 = ((GameObject)goal.Data).transform.position;

        return Vector3.Distance(pos1, pos2);
    }

    protected void Update() { }
}
