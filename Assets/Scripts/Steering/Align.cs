using UnityEngine;
using System.Collections;

public class Align : SteeringBehaviour {

    [SerializeField]
    protected float maxAngularAcceleration;

    [SerializeField]
    protected float targetAngle;

    [SerializeField]
    protected float slowAngle;

    [SerializeField]
    protected float timeToTarget;

    public override float AngularAcceleration {
        get {
            float direction = Mathf.Sign(Vector3.Dot(Vector3.up, Vector3.Cross(transform.forward, target.forward)));
            float angle = Vector3.Angle(transform.forward, target.forward);
            
            if (angle < targetAngle)
                return 0.0f;

            float targetAngularSpeed;
            if (angle > slowAngle)
                targetAngularSpeed = MaxAngularSpeed;
            else
                targetAngularSpeed = MaxAngularSpeed * angle / slowAngle;

            float targetAngularVelocity = targetAngularSpeed * direction;

            float angularAcceleration = targetAngularVelocity - AngularVelocity;
            angularAcceleration /= timeToTarget;

            angularAcceleration = Mathf.Clamp(angularAcceleration, -maxAngularAcceleration, maxAngularAcceleration);

            return angularAcceleration;
        }
    }

}
