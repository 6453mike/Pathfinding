using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
    void OnMouseOver() {
        // Set the goal node
        if (Input.GetMouseButtonDown(0)) {
            PathController.instance.Goal = GraphController.instance.GetNode(transform.position.ToString());
            PathController.instance.DrawPath();
        }

        // Set the start node
        if (Input.GetMouseButtonDown(1)) {
            PathController.instance.Start = GraphController.instance.GetNode(transform.position.ToString());
            PathController.instance.DrawPath();
        }
    }
}
