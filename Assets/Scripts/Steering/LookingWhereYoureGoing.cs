using UnityEngine;
using System.Collections;

public class LookingWhereYoureGoing : Align {

    public override float AngularAcceleration {
        get {
            if (Velocity.magnitude == 0.0f)
                return 0.0f;

            Target.rotation = Quaternion.LookRotation(Velocity);
            
            return base.AngularAcceleration;
        }
    }

    protected override void Awake() {
        base.Awake();

        target = new GameObject("Align").transform;
        target.parent = transform;
    }

}
