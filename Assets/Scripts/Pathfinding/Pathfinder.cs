using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class Pathfinder : MonoBehaviour {
    public abstract List<PathfinderNode> FindPath(Node start, Node goal, Graph graph);
    public abstract void ShowVisuals();
}
