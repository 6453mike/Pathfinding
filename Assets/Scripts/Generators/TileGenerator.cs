using UnityEngine;
using System.Collections;

public class TileGenerator : MonoBehaviour {
    // The size which to scale a tile
    [SerializeField]
    private float tileSize;

    // The space inbetween tiles
    [SerializeField]
    private float tilePadding;

    // Nodes will be flood-filled beginning from this location
    [SerializeField]
    private Transform seed;

    // An empty gameobject to store the generated nodes
    [SerializeField]
    private Transform nodeContainer;

    // A flag enum to help with composite directions
    // (Eg. Top-left = Direction.Top | Direction.Left)
    [System.Flags]
    private enum Direction { Top = 1, Bottom = 2, Left = 4, Right = 8 };

    // This padding should be greater than zero. It is to prevent collision
    // between nodes when they are being placed.
    private const float COLLISION_PADDING = 0.01f;

    /// <summary>
    /// Recursive method that flood-fills nodes onto the map beginning
    /// from the seed location.
    /// </summary>
    /// <param name="position">The global position where the newly created node will be placed.</param>
    /// <param name="sourceNode">The previously created node from the caller.</param>
    private void CreateNode(Vector3 position, Node sourceNode = null) {
        // The radius of the sphere that is used for collision detection
        float sphereRadius = (tileSize / 2.0f) + (tilePadding - COLLISION_PADDING);

        // If the sphere collides with a wall, then don't create a node
        Collider[] wallColliders = Physics.OverlapSphere(position, sphereRadius, LayerMask.GetMask("Walls"));
        if (wallColliders.Length > 0) return;

        // If the sphere collides with a node, then create an edge to that node; otherwise create a new node
        Node node = null;
        Collider[] nodeColliders = Physics.OverlapSphere(position, sphereRadius, LayerMask.GetMask("Nodes"));
        if (nodeColliders.Length > 0) {
            // The collided node's position is the key for retrieving the Node object from the graph
            node = GraphController.instance.GetNode(nodeColliders[0].transform.position.ToString());
        } else {
            GameObject newObject = (GameObject)Instantiate(MyResources.Load("Node"), position, Quaternion.identity);
            newObject.transform.localScale = new Vector3(tileSize, newObject.transform.localScale.y, tileSize);
            newObject.transform.parent = nodeContainer;

            // The position is used as a key to for retrieval if needed
            node = new Node(position.ToString(), newObject);
        }

        // Only create edges if there is a source node to connect to
        if (sourceNode != null) {
            float cost = Vector3.Distance(((GameObject)sourceNode.Data).transform.position, position);
            CreateEdge(sourceNode, node, cost);
        }

        // If the node existed before this method call, then it is a fact that the
        // nodes around it were also created
        if (nodeColliders.Length > 0) return;

        // Make the recursive calls to create the neighbouring nodes
        CreateNode(MovePosition(position, Direction.Top), node);
        CreateNode(MovePosition(position, Direction.Top | Direction.Right), node);
        CreateNode(MovePosition(position, Direction.Right), node);
        CreateNode(MovePosition(position, Direction.Right | Direction.Bottom), node);
        CreateNode(MovePosition(position, Direction.Bottom), node);
        CreateNode(MovePosition(position, Direction.Bottom | Direction.Left), node);
        CreateNode(MovePosition(position, Direction.Left), node);
        CreateNode(MovePosition(position, Direction.Left | Direction.Top), node);
    }

    private void CreateEdge(Node sourceNode, Node targetNode, float cost) {
        GraphController.instance.AddEdge(sourceNode, targetNode, cost);
    }

    /// <summary>
    /// Returns a position in a direction relative to the given position .
    /// </summary>
    /// <param name="position"></param>
    /// <param name="direction"></param>
    /// <returns>A new position</returns>
    private Vector3 MovePosition(Vector3 position, Direction direction) {
        Vector3 vectorDirection = Vector3.zero;

        if ((direction & Direction.Top) == Direction.Top) vectorDirection += Vector3.forward;
        if ((direction & Direction.Bottom) == Direction.Bottom) vectorDirection -= Vector3.forward;
        if ((direction & Direction.Right) == Direction.Right) vectorDirection += Vector3.right;
        if ((direction & Direction.Left) == Direction.Left) vectorDirection -= Vector3.right;

        return position + (tileSize + tilePadding) * vectorDirection;
    }

    protected void Start() {
        CreateNode(seed.position);
        GraphController.instance.ResetColor();
        GameObject.FindGameObjectWithTag("Pathfinder").SendMessage("OnNodesReady", SendMessageOptions.DontRequireReceiver);
        UIController.instance.SetNumberOfNodes(GraphController.instance.Graph.Nodes.Count);
        UIController.instance.SetGraphType("Tile");
    }
}
