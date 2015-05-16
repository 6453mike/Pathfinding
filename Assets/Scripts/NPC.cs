using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class NPC : MonoBehaviour {
    private SteeringAgent steeringAgent;
    private Arrive arrive;

    private bool isGoalReached;

    private List<PathfinderNode> path;
    private int currentTargetIndex;

    public IEnumerator FollowPath(List<PathfinderNode> path) {
        this.path = path;
        isGoalReached = false;

        currentTargetIndex = 0;

        while (!isGoalReached) {
            arrive.Target = ((GameObject)path[currentTargetIndex].Data).transform;
            yield return 0;
        }
    }

    public void TargetReached(Transform target) {
        // Check if we have reached the goal
        if (currentTargetIndex == path.Count - 1) {
            isGoalReached = true;
            steeringAgent.ResetVelocity();
            steeringAgent.ResetAngularVelocity();
        } else {
            if (target == ((GameObject)path[currentTargetIndex].Data).transform)
                currentTargetIndex++; 
        }
    }

    protected void Awake() {
        steeringAgent = gameObject.GetComponent<SteeringAgent>();
        arrive = gameObject.GetComponent<Arrive>();

        isGoalReached = false;
    }
}
