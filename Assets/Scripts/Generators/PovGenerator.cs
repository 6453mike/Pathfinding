using UnityEngine;
using System.Collections;

public class PovGenerator : MonoBehaviour {
    private GameObject[] nodes;

    private void BuildGraph() {
        foreach (GameObject n in nodes) {
            Node node = new Node(n.transform.position.ToString(), n);
            foreach (GameObject o in nodes) {
                if (n == o) continue;

                float distance = Vector3.Distance(n.transform.position, o.transform.position);

                if (Physics.Raycast(n.transform.position, o.transform.position - n.transform.position, distance, LayerMask.GetMask("Walls"))) continue;

                Node otherNode = GraphController.instance.GetNode(o.transform.position.ToString());
                
                if (otherNode == null)
                    otherNode = new Node(o.transform.position.ToString(), o);

                CreateEdge(node, otherNode, distance);
                CreateEdge(otherNode, node, distance);
            }
        }
    }

    private void CreateEdge(Node sourceNode, Node targetNode, float cost) {
        GraphController.instance.AddEdge(sourceNode, targetNode, cost);
    }

    protected void Awake() {
        nodes = GameObject.FindGameObjectsWithTag("Node");
    }

    protected void Start() {
        BuildGraph();
        GameObject.FindGameObjectWithTag("Pathfinder").SendMessage("OnNodesReady", SendMessageOptions.DontRequireReceiver);
        UIController.instance.SetNumberOfNodes(GraphController.instance.Graph.Nodes.Count);
        UIController.instance.SetGraphType("PoV");
    }
}
