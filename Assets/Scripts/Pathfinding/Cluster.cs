using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class Cluster : Heuristic {
    private Dictionary<Vector3, GameObject> nodeToCluster;
    private Dictionary<Vector3, List<Node>> clusterToNodes;

    private GameObject[] clusterObjects;

    struct Connection {
        public Node node1;
        public Node node2;
        public float cost;

        public Connection(Node node1, Node node2, float cost) {
            this.node1 = node1;
            this.node2 = node2;
            this.cost = cost;
        }
    }

    private Dictionary<string, Connection> shortestConnections;

    private Dictionary<string, float> lookupTable;

    private Graph clusterGraph;
    private Graph mapGraph;

    public override float CalculateHeuristic(Node currentNode, Node goal, Graph graph) {
        Vector3 position1 = ((GameObject)currentNode.Data).transform.position;
        Vector3 position2 = ((GameObject)goal.Data).transform.position;
        GameObject cluster1 = nodeToCluster[position1];
        GameObject cluster2 = nodeToCluster[position2];

        if (cluster1 == cluster2) return Vector3.Distance(position1, position2);

        string key = GetConnectionKey(cluster1, cluster2);

        return lookupTable[key];
    }

    private void GenerateClusterGraph() {
        foreach (Connection c in shortestConnections.Values) {
            GameObject clusterObject1 = nodeToCluster[((GameObject)c.node1.Data).transform.position];
            GameObject clusterObject2 = nodeToCluster[((GameObject)c.node2.Data).transform.position];

            string key1 = clusterObject1.transform.position.ToString();
            string key2 = clusterObject2.transform.position.ToString();

            Node clusterNode1 = (clusterGraph.Contains(key1)) ? clusterGraph.Nodes[key1] : new Node(key1, clusterObject1);
            Node clusterNode2 = (clusterGraph.Contains(key2)) ? clusterGraph.Nodes[key2] : new Node(key2, clusterObject2);
    
            clusterGraph.AddNode(clusterNode1);
            clusterGraph.AddNode(clusterNode2);
            clusterGraph.AddDirectedEdge(clusterNode1, clusterNode2, c.cost);
            clusterGraph.AddDirectedEdge(clusterNode2, clusterNode1, c.cost);
        }
    }

    private void AssignNodesToClusters() {
        foreach (Node n in mapGraph.Nodes) {
            foreach (GameObject clusterObject in clusterObjects) {
                if (IsNodeInCluster(n, clusterObject)) {
                    nodeToCluster[((GameObject)n.Data).transform.position] = clusterObject;

                    if (!clusterToNodes.ContainsKey(clusterObject.transform.position))
                        clusterToNodes[clusterObject.transform.position] = new List<Node>();

                    clusterToNodes[clusterObject.transform.position].Add(n);
                    break;
                }
            }
        }
    }

    private void GenerateClusterConnections() {
        foreach (Node n in mapGraph.Nodes) {
            foreach (EdgeToNeighbour e in n.Neighbours) {
                GameObject cluster1 = nodeToCluster[((GameObject)n.Data).transform.position];
                GameObject cluster2 = nodeToCluster[((GameObject)e.Neighbour.Data).transform.position];

                if (cluster1 != cluster2) {
                    string key = GetConnectionKey(cluster1, cluster2);

                    if (!shortestConnections.ContainsKey(key) ||
                        e.Cost < shortestConnections[key].cost) {
                        shortestConnections[key] = new Connection(n, e.Neighbour, e.Cost);
                    }
                }
            }
        }
    }

    private void GenerateLookupTable() {
        List<Node> checkedNodes = new List<Node>();
        foreach (Node n in clusterGraph.Nodes) {
            checkedNodes.Add(n);

            foreach (Node on in clusterGraph.Nodes) {
                string key = GetConnectionKey((GameObject)n.Data, (GameObject)on.Data);

                if (n == on) continue;
                if (checkedNodes.Contains(on)) continue;

                lookupTable[key] = Dijkstra(n, on, clusterGraph);
            }
        }
    }

    private bool IsNodeInCluster(Node node, GameObject cluster) {
        GameObject n = (GameObject)node.Data;
        Vector3 nPos = n.transform.position;
        Vector3 min = cluster.collider.bounds.min;
        Vector3 max = cluster.collider.bounds.max;

        if (nPos.x > min.x && nPos.x < max.x &&
            nPos.y > min.y && nPos.y < max.y &&
            nPos.z > min.z && nPos.z < max.z)
            return true;

        return false;
    }

    private string GetConnectionKey(GameObject cluster1, GameObject cluster2) {
        string key;
        if (cluster1.transform.position.magnitude < cluster2.transform.position.magnitude) {
            key = cluster1.transform.position.ToString() + cluster2.transform.position.ToString();
        } else {
            key = cluster2.transform.position.ToString() + cluster1.transform.position.ToString();
        }

        return key;
    }

    public float Dijkstra(Node start, Node goal, Graph graph) {
        PriorityQueue<PathfinderNode>  priority = new PriorityQueue<PathfinderNode>();
        Dictionary<string, PathfinderNode>  open = new Dictionary<string, PathfinderNode>();
        Dictionary<string, PathfinderNode>  closed = new Dictionary<string, PathfinderNode>();

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
                return lowestCostNode.CostSoFar;

            open.Remove(lowestCostNode.Key);
            closed[lowestCostNode.Key] = lowestCostNode;

            for (int i = 0; i < currentNode.Neighbours.Count; i++) {
                successorNode = new PathfinderNode(currentNode.Neighbours[i].Neighbour);

                if (closed.ContainsKey(successorNode.Key)) continue;

                successorNode.Parent = lowestCostNode;
                successorNode.CostSoFar = lowestCostNode.CostSoFar + currentNode.Neighbours[i].Cost;

                if (!open.ContainsKey(successorNode.Key) ||
                    open[successorNode.Key].TotalCost > successorNode.TotalCost) {
                    open[successorNode.Key] = successorNode;
                    priority.Enqueue(successorNode);
                }
            }
        }

        return 0.0f;
    }

    public void OnNodesReady() {
        mapGraph = GraphController.instance.Graph;

        AssignNodesToClusters();
        GenerateClusterConnections();
        GenerateClusterGraph();
        GenerateLookupTable();
    }

    protected void Awake() {
        clusterGraph = new Graph();

        nodeToCluster = new Dictionary<Vector3, GameObject>();
        clusterToNodes = new Dictionary<Vector3, List<Node>>();

        shortestConnections = new Dictionary<string, Connection>();
        lookupTable = new Dictionary<string, float>();

        clusterObjects = GameObject.FindGameObjectsWithTag("Cluster");
    }

    protected void Update() { }

    protected void OnDrawGizmos() {
        if (clusterGraph == null) return;
        foreach (Node n in clusterGraph.Nodes) {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(((GameObject)n.Data).transform.position, new Vector3(4, 4, 4));
            Vector3 start = ((GameObject)n.Data).transform.position;
            foreach (EdgeToNeighbour b in n.Neighbours) {
                Gizmos.DrawLine(start, ((GameObject)b.Neighbour.Data).transform.position);
            }
        }
        Gizmos.color = Color.red;
        foreach (Connection c in shortestConnections.Values) {
            Gizmos.DrawLine(((GameObject)c.node1.Data).transform.position, ((GameObject)c.node2.Data).transform.position);
        }
    }
}
