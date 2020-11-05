using Assets.Characters.MovementBehaviors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceBehavior : RotationBehavior
{
    //---Stats---
    [SerializeField] protected float _slowDownAngle = 35.0f;

    //---Variables---
    [SerializeField] protected Vector3 _target = Vector3.zero;

    //---Public---
    public virtual Vector3 Target { get { return _target; } set { _target = value; } }

    public override RotationOutput HandleRotation(MovementAgent agent)
    {
        RotationOutput output = new RotationOutput { IsValid = true, DesiredAngularVelocity = 0.0f };

        Vector2 agentForward = new Vector2(agent.transform.forward.x, agent.transform.forward.z);
        Vector2 targetPos = new Vector2(_target.x, _target.z);
        Vector2 toTarget = new Vector2(_target.x - agent.transform.position.x, _target.z - agent.transform.position.z);

        float angle = Vector2.SignedAngle(toTarget, agentForward);

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
