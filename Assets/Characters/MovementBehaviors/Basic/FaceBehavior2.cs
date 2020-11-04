using Assets.Characters.MovementBehaviors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceBehavior2 : RotationBehavior
{
    //---Variables---
    [SerializeField] protected Vector3 _target = Vector3.zero;
    [SerializeField] protected float _slowDownAngle = 35.0f;

    //---Public---
    public Vector3 Target { get { return _target; } set { _target = value; } }


    public override RotationOutput HandleRotation(MovementAgent agent)
    {
        RotationOutput output = new RotationOutput { IsValid = true, DesiredAngularVelocity = 0.0f };

        float angle = Vector3.SignedAngle(agent.transform.forward, _target - agent.transform.position, Vector3.up);

        //no rotation needed if already facing target
        if (angle == 0.0f)
            return output;

        float unsignedAngle = Mathf.Abs(angle);
        float sign = Mathf.Sign(angle);

        output.DesiredAngularVelocity = agent.MaxAngularVelocity * sign;

        //slow down if close to target
        if (_slowDownAngle > 0.0f && unsignedAngle < _slowDownAngle)
            output.DesiredAngularVelocity *= unsignedAngle / _slowDownAngle;

        return output;
    }
}
