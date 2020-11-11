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
    [SerializeField] private Vector2 _target = Vector3.zero;
    [SerializeField] protected Transform _lockedTarget = null;


    //---Public---
    public Vector2 Target2D
    {
        get { return _target; }
        set
        {
            _target.x = value.x;
            _target.y = value.y;
        }
    }

    public Vector3 Target3D
    {
        get { return new Vector3(_target.x, transform.position.y, _target.y); }
        set { _target.x = value.x; _target.y = value.z; }
    }

    public Transform LockedTarget { get { return _lockedTarget; } set { _lockedTarget = value; } }

    void Update()
    {
        if (_lockedTarget)
        {
            _target.x = _lockedTarget.position.x;
            _target.y = _lockedTarget.position.z;
        }
    }

    void OnEnable()
    {
        _target = transform.position;
    }

    public override MovementOutput HandleMovement(MovementAgent agent)
    {
        MovementOutput output = new MovementOutput { IsValid = true, ShouldJump = false };

        Vector2 agentPos = new Vector2(agent.transform.position.x, agent.transform.position.z);

        Vector2 vel = _target - agentPos;
        Vector3 vel3D = new Vector3(vel.x, 0.0f, vel.y);
        vel3D = agent.transform.InverseTransformDirection(vel3D);
        vel.x = vel3D.x;
        vel.y = vel3D.z;

        //recover lost velocity of going backwards
        if (vel.y < 0.0f
            && !agent.CanMoveBackwards)
            vel.y = -vel.y * _recoverLostVelocityRatio;

       if(!agent.CanMoveSideways)
        {
            float recover = vel.x * _recoverLostVelocityRatio;
            vel.x -= recover;
            vel.y += Mathf.Abs(recover);
        }

        vel = vel.normalized * agent.MaxVelocity;
        output.DesiredVelocity = new Vector2(vel.x, vel.y);

        if (agent.AutoRotation)
            agent.AutoRotation.Target2D = _target;

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