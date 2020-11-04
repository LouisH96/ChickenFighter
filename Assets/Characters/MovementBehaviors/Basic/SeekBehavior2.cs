using Assets.Characters.MovementBehaviors;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class SeekBehavior2 : MovementBehavior2
{
    //---Stats---

    //---Variables---
    [SerializeField] protected Vector3 _target = Vector3.zero;
    [SerializeField] protected Transform _lockedTarget = null;

    //---Public---
    public Vector3 Target { get { return _target; } set { _target = value; } }
    public Transform LockedTarget { get { return _lockedTarget; } set { _lockedTarget = value; } }

    void Update()
    {
        if (_lockedTarget)
            _target = _lockedTarget.position;
    }

    public override MovementOutput HandleMovement(MovementAgent agent)
    {
        MovementOutput output = new MovementOutput { IsValid = true, ShouldJump = false };

        Vector3 v = _target - agent.transform.position;
         v = agent.transform.InverseTransformDirection(v);
        v = v.normalized * agent.MaxAngularVelocity; ;

        output.DesiredVelocity = new Vector2(v.x, v.z);

        return output;
    }
}
