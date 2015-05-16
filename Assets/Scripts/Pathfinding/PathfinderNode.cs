using UnityEngine;
using System;
using System.Collections;

public class PathfinderNode : Node, IComparable<PathfinderNode> {
    private PathfinderNode parent;

    private float costSoFar;
    private float heuristic;

    public PathfinderNode(Node node) : this(node.Key, node.Data) { }

    public PathfinderNode(string key, object data) : base(key, data) {
        costSoFar = 0.0f;
        heuristic = 0.0f;
    }

    public PathfinderNode Parent {
        get {
            return parent;
        }
        set {
            parent = value;
        }
    }

    public float CostSoFar {
        get {
            return costSoFar;
        }
        set {
            costSoFar = value;
        }
    }

    public float Heuristic {
        get {
            return heuristic;
        }
        set {
            heuristic = value;
        }
    }

    public float TotalCost {
        get {
            return costSoFar + heuristic;
        }
    }

    public int CompareTo(PathfinderNode other) {
        if (TotalCost < other.TotalCost) return -1;
        else if (TotalCost > other.TotalCost) return 1;
        else return 0;
    }
}
