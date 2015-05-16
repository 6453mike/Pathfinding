using UnityEngine;
using System.Collections;

public class SteeringAgent : MonoBehaviour {
    [SerializeField]
    private float maxSpeed;

    [SerializeField]
    private float maxAngularSpeed;

    private SteeringBehaviour[] behaviours;

    public Vector3 Velocity { get; private set; }
    public float AngularVelocity { get; private set; }

    public float MaxSpeed {
        get { 
            return maxSpeed;
        }
    }

    public float MaxAngularSpeed {
        get {
            return maxAngularSpeed;
        }
    }

    public void ResetVelocity() {
        Velocity = Vector3.zero;
    }

    public void ResetAngularVelocity() {
        AngularVelocity = 0.0f;
    }

    protected void Awake() {
        behaviours = gameObject.GetComponents<SteeringBehaviour>();
    }

    protected void FixedUpdate() {
        UpdateVelocity(Time.deltaTime);
        UpdateAngularVelocity(Time.deltaTime);

        UpdatePosition(Time.deltaTime);
        UpdateRotation(Time.deltaTime);
    }

    private void UpdateVelocity(float deltaTime) {
        Vector3 totalAcceleration = Vector3.zero;
        foreach (SteeringBehaviour b in behaviours) {
            totalAcceleration += b.Acceleration;
        }

        Velocity = Velocity + totalAcceleration * deltaTime;
        Velocity = Vector3.ClampMagnitude(Velocity, maxSpeed);
    }

    private void UpdateAngularVelocity(float deltaTime) {
        float totalAngularAcceleration = 0.0f;
        foreach (SteeringBehaviour b in behaviours) {
            totalAngularAcceleration += b.AngularAcceleration;
        }

        AngularVelocity = AngularVelocity + totalAngularAcceleration * deltaTime;
        AngularVelocity = Mathf.Clamp(AngularVelocity, -maxAngularSpeed, maxAngularSpeed);
    }

    private void UpdatePosition(float deltaTime) {
        transform.Translate(Velocity * deltaTime, Space.World);
    }

    protected void UpdateRotation(float deltaTime) {
        transform.Rotate(Vector3.up, AngularVelocity * deltaTime, Space.World);
    }
}
