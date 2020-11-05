using Assets.Characters.MovementBehaviors;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class SeekBehavior : MovementBehavior
{
    //---Stats---
    [SerializeField] protected float _recoverLostVelocityRatio = 0.0f;

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

        Vector3 vel = _target - agent.transform.position;
        vel = agent.transform.InverseTransformDirection(vel);

        //recover lost velocity of going backwards
        if (vel.z < 0.0f
            && !agent.CanMoveBackwards)
            vel.z = -vel.z * _recoverLostVelocityRatio;

       if(!agent.CanMoveSideways)
        {
            float recover = vel.x * _recoverLostVelocityRatio;
            vel.x -= recover;
            vel.z += Mathf.Abs(recover);
        }

        vel = vel.normalized * agent.MaxVelocity;
        output.DesiredVelocity = new Vector2(vel.x, vel.z);

        if (agent.AutoRotation)
            agent.AutoRotation.Target = _target;

        return output;
    }

    protected virtual void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR

        Debug.DrawLine(transform.position, _target);

#endif
    }
}