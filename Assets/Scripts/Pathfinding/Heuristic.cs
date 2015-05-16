using UnityEngine;
using System.Collections;

public abstract class Heuristic : MonoBehaviour {
    public abstract float CalculateHeuristic(Node currentNode, Node goal, Graph graph);
}
