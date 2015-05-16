using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {
    public static InputController instance;

    private Cluster cluster;
    private Dijkstra dijkstra;
    private Euclidean euclidean;

    private GameObject pathfinder;

    protected void Awake() {
        instance = this;

        pathfinder = GameObject.FindGameObjectWithTag("Pathfinder");
        cluster = pathfinder.GetComponent<Cluster>();
        dijkstra = pathfinder.GetComponent<Dijkstra>();
        euclidean = pathfinder.GetComponent<Euclidean>();
    }

    protected void OnDestroy() {
        if (instance != null) {
            instance = null;
        }
    }

    protected void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneController.instance.SwitchScene("MainMenu");
        }

        if (Input.GetKeyDown(KeyCode.D)) {
            dijkstra.enabled = true;
            cluster.enabled = false;
            euclidean.enabled = false;
            pathfinder.SendMessage("UpdateHeuristic", SendMessageOptions.DontRequireReceiver);
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            euclidean.enabled = true;
            cluster.enabled = false;
            dijkstra.enabled = false;
            pathfinder.SendMessage("UpdateHeuristic", SendMessageOptions.DontRequireReceiver);
        }

        if (Input.GetKeyDown(KeyCode.C)) {
            cluster.enabled = true;
            dijkstra.enabled = false;
            euclidean.enabled = false;
            pathfinder.SendMessage("UpdateHeuristic", SendMessageOptions.DontRequireReceiver);
        }
    }
}
