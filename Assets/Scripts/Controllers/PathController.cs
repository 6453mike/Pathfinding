using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PathController : MonoBehaviour {
    public static PathController instance;

    [SerializeField]
    private Color startNodeColor;

    [SerializeField]
    private Color goalNodeColor;

    [SerializeField]
    private Color pathNodeColor;

    [SerializeField]
    private Color pathEdgeColor;

    [SerializeField]
    private float pathEdgeThickness;

    private Pathfinder pathFinder;
    private LineRenderer lineRenderer;

    private Node start;
    private Node goal;
    private List<PathfinderNode> path;

    private NPC npc;
    private IEnumerator currentCoroutine;

    public Node Start {
        get {
            return start;
        }
        set {
            start = value;
            UpdatePath();
        }
    }

    public Node Goal {
        get {
            return goal;
        }
        set {
            goal = value;
            UpdatePath();
        }
    }

    public void UpdatePath() {
        if (start == null) return;

        if (goal == null) {
            path = new List<PathfinderNode>();
            path.Add(new PathfinderNode(start));
        } else {
            path = pathFinder.FindPath(start, goal, GraphController.instance.Graph);
        }

        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        currentCoroutine = npc.FollowPath(path);
        StartCoroutine(currentCoroutine);
    }

    public void DrawPath() {
        // Reset the graph color
        GraphController.instance.ResetColor();

        if (path != null) {
            pathFinder.ShowVisuals();

            lineRenderer.SetVertexCount(path.Count);
            for (int i = 0; i < path.Count; i++) {
                ((GameObject)path[i].Data).renderer.material.color = pathNodeColor;
                lineRenderer.SetPosition(i, ((GameObject)path[i].Data).transform.position + new Vector3(0.0f, 0.2f, 0.0f));
            }
        }

        if (Goal != null) ((GameObject)Goal.Data).renderer.material.color = goalNodeColor;
        if (Start != null) ((GameObject)Start.Data).renderer.material.color = startNodeColor;
    }

    protected void Awake() {
        instance = this;

        GameObject pfObject = GameObject.FindGameObjectWithTag("Pathfinder");
        pathFinder = pfObject.GetComponent<Pathfinder>();
        lineRenderer = pfObject.GetComponentInChildren<LineRenderer>();
        lineRenderer.material.color = pathEdgeColor;
        lineRenderer.SetColors(pathEdgeColor, pathEdgeColor);
        lineRenderer.SetWidth(pathEdgeThickness, pathEdgeThickness);

        npc = GameObject.FindGameObjectWithTag("NPC").GetComponent<NPC>();
    }

    protected void OnDestroy() {
        if (instance != null) {
            instance = null;
        }
    }

    protected void OnDrawGizmos() {
        if (path == null) return;
        Gizmos.color = Color.blue;
        foreach (PathfinderNode n in path) {
            Gizmos.DrawCube(((GameObject)n.Data).transform.position, new Vector3(1.0f, 1.0f, 1.0f));
        }
    }
}
