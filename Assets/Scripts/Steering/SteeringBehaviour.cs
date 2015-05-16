using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SteeringAgent))]
public class SteeringBehaviour : MonoBehaviour {
    [SerializeField]
    protected Transform target;

    protected SteeringAgent steeringAgent;

    public virtual Transform Target {
        get {
            return target;
        }
        set {
            target = value;
        }
    }

    public virtual Vector3 Acceleration { 
        get {
            return Vector3.zero;
        }
    }

    public virtual float AngularAcceleration {
        get {
            return 0.0f;
        }
    }

    protected float MaxSpeed {
        get {
            return steeringAgent.MaxSpeed;
        }
    }

    protected float MaxAngularSpeed {
        get {
            return steeringAgent.MaxAngularSpeed;
        }
    }

    protected Vector3 Velocity {
        get {
            return steeringAgent.Velocity;
        }
    }

    protected float AngularVelocity {
        get {
            return steeringAgent.AngularVelocity;
        }
    }

    protected virtual void Awake() {
        steeringAgent = gameObject.GetComponent<SteeringAgent>();
    }
}
