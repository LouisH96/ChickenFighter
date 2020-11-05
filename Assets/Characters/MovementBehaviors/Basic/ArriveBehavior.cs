using Assets.Characters.MovementBehaviors;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ArriveBehavior : SeekBehavior
{
    //---Stats---
    [SerializeField] private float _slowdownRadius = 1.0f;
    [SerializeField] private float _slowestVelocityRatio = 0.2f;
    [SerializeField] private float _stopRadius = 0.25f;

    //---Public---
    public float StopRadius { get{ return _stopRadius; } set { _stopRadius = value; } }

    public override MovementOutput HandleMovement(MovementAgent agent)
    {
        MovementOutput output = base.HandleMovement(agent);

        float sqrDistance = (agent.transform.position - Target).sqrMagnitude;

        if (sqrDistance <= _stopRadius * _stopRadius)
        {
            output.DesiredVelocity = Vector3.zero;
            output.IsValid = false;
            return output;
        }
        else
        if (_slowdownRadius > 0.0f && sqrDistance <= _slowdownRadius * _slowdownRadius)
            output.DesiredVelocity *= (sqrDistance / (_slowdownRadius * _slowdownRadius))
                * (1.0f - _slowestVelocityRatio) + _slowestVelocityRatio;

        return output;
    }

    public bool IsTargetReached(MovementAgent agent)
    {
        return (agent.transform.position - Target).sqrMagnitude <= _stopRadius * _stopRadius;
    }

#if UNITY_EDITOR
    protected override void OnDrawGizmosSelected()
    {
        if (!ShowDebugInfo)
            return;

        Handles.color = Color.red;
        Handles.DrawWireDisc(Target, Vector3.up, _stopRadius);

        if (_slowdownRadius > 0.0f)
        {
            Handles.color = new Color(1.0f, 0.5f, 0.0f);
            Handles.DrawWireDisc(Target, Vector3.up, _slowdownRadius);
        }
    }
#endif
}
