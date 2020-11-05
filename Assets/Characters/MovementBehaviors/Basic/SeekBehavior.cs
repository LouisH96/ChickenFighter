using Assets.Characters.MovementBehaviors;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Assertions.Must;
using UnityEngine.Rendering;

public class SeekBehavior : MovementBehavior
{
    //---Stats---
    [SerializeField] protected float _recoverLostVelocityRatio = 0.0f;

    //---Variables---
    [SerializeField] private Vector3 _target = Vector3.zero;
    [SerializeField] protected Transform _lockedTarget = null;


    //---Public---
    public Vector3 Target
    {
        get { return _target; }
        set
        {
            _target.x = value.x;
            _target.z = value.z;
        }
    }
    public Transform LockedTarget { get { return _lockedTarget; } set { _lockedTarget = value; } }

    void Update()
    {
        if (_lockedTarget)
        {
            _target.x = _lockedTarget.position.x;
            _target.z = _lockedTarget.position.z;
        }
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
        if (!ShowDebugInfo)
            return;

        Debug.DrawLine(transform.position, _target, Color.green);

#endif
    }
}