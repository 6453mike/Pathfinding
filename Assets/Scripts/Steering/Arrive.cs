using UnityEngine;
using System.Collections;

public class Arrive : SteeringBehaviour {
    [SerializeField]
    protected float maxAcceleration;

    [SerializeField]
    protected float targetRadius;

    [SerializeField]
    protected float slowRadius;

    [SerializeField]
    protected float timeToTarget;

    public override Vector3 Acceleration {
        get {
            if (!target) return Vector3.zero;

            Vector3 direction = target.position - transform.position;
            float distance = direction.magnitude;
            direction.Normalize();

            if (distance <= targetRadius) {
                gameObject.SendMessage("TargetReached", target, SendMessageOptions.DontRequireReceiver);
                return Vector3.zero;
            }

            float targetSpeed;
            if (distance > slowRadius)
                targetSpeed = MaxSpeed;
            else
                targetSpeed = MaxSpeed * distance / slowRadius;

            Vector3 targetVelocity = targetSpeed * direction;

            Vector3 acceleration = targetVelocity - Velocity;
            acceleration /= timeToTarget;

            acceleration = Vector3.ClampMagnitude(acceleration, maxAcceleration);

            return acceleration;
        }
    }
}
