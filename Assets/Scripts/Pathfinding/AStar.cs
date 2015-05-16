using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AStar : Pathfinder
{
    [SerializeField]
    private Color openNodesColor;

    [SerializeField]
    private Color closedNodesColor;

    private PriorityQueue<PathfinderNode> priority;
    private Dictionary<string, PathfinderNode> open;
    private Dictionary<string, PathfinderNode> closed;

    private Heuristic heuristic;

    public override List<PathfinderNode> FindPath(Node start, Node goal, Graph graph) {
        priority = new PriorityQueue<PathfinderNode>();
        open = new Dictionary<string, PathfinderNode>();
        closed = new Dictionary<string, PathfinderNode>();

        PathfinderNode startNode = new PathfinderNode(start);
        priority.Enqueue(startNode);
        open[startNode.Key] = startNode;

        Node currentNode;
        PathfinderNode lowestCostNode;
        PathfinderNode successorNode;

        while (priority.Count() > 0) {
            lowestCostNode = priority.Dequeue();
            currentNode = graph.Nodes[lowestCostNode.Key];

            if (lowestCostNode.Key == goal.Key)
                return ConstructPath(lowestCostNode);

            open.Remove(lowestCostNode.Key);
            closed[lowestCostNode.Key] = lowestCostNode;

            for (int i = 0; i < currentNode.Neighbours.Count; i++) {
                successorNode = new PathfinderNode(currentNode.Neighbours[i].Neighbour);

                if (closed.ContainsKey(successorNode.Key)) continue;

                successorNode.Parent = lowestCostNode;
                successorNode.CostSoFar = lowestCostNode.CostSoFar + currentNode.Neighbours[i].Cost;
                successorNode.Heuristic = heuristic.CalculateHeuristic(currentNode, goal, graph);

                if (!open.ContainsKey(successorNode.Key) ||
                    open[successorNode.Key].TotalCost > successorNode.TotalCost) {
                    open[successorNode.Key] = successorNode;
                    priority.Enqueue(successorNode);
                }
            }
        }

        return null;
    }

    private List<PathfinderNode> ConstructPath(PathfinderNode endNode) {
        List<PathfinderNode> path = new List<PathfinderNode>();
        while (endNode != null) {
            path.Add(endNode);
            endNode = endNode.Parent;
        }

        path.Reverse();

        return path;
    }

    public void UpdateHeuristic() {
        Heuristic[] heuristics = gameObject.GetComponents<Heuristic>();
        foreach (Heuristic h in heuristics) {
            if (h.enabled) {
                UIController.instance.SetHeuristicType(h.GetType().ToString());
                heuristic = h;
            }
        }
    }

    public override void ShowVisuals() {
        DrawOpenNodes();
        DrawClosedNodes();
    }

    private void DrawClosedNodes() {
        if (closed != null) {
            foreach (PathfinderNode n in closed.Values) {
                ((GameObject)n.Data).renderer.material.color = closedNodesColor;
            }
        }
    }

    private void DrawOpenNodes() {
        if (open != null) {
            foreach (PathfinderNode n in open.Values) {
                ((GameObject)n.Data).renderer.material.color = openNodesColor;
            }
        }
    }

    protected void Start() {
        UpdateHeuristic();
    }
}
