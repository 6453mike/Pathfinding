using UnityEngine;
using System.Collections;

public class Face : Align {

    protected Transform faceTarget;

    public override Transform Target {
        get {
            return faceTarget;
        }
        set {
            faceTarget = value;
        }
    }

    public override float AngularAcceleration {
        get {
            Vector3 direction = faceTarget.position - transform.position;
            direction.Normalize();

            target.rotation = Quaternion.LookRotation(direction);
            
            return base.AngularAcceleration;
        }
    }

    protected override void Awake() {
        base.Awake();

        faceTarget = target;
        target = new GameObject("Align").transform;
        target.parent = transform;
    }

}
