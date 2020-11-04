using Assets.Characters.MovementBehaviors;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ArriveBehavior : SeekBehavior2
{
    //---Stats---
    [SerializeField] private float _slowdownRadius = 1.0f;
    [SerializeField] private float _slowestVelocityRatio = 0.2f;
    [SerializeField] private float _stopRadius = 0.25f;

    //---Public---

    public override MovementOutput HandleMovement(MovementAgent agent)
    {
        MovementOutput output = base.HandleMovement(agent);

        float sqrDistance = (agent.transform.position - _target).sqrMagnitude;

        if (sqrDistance <= _stopRadius * _stopRadius)
            output.DesiredVelocity = Vector3.zero;
        else
        if (_slowdownRadius > 0.0f && sqrDistance <= _slowdownRadius * _slowdownRadius)
            output.DesiredVelocity *= (sqrDistance / (_slowdownRadius * _slowdownRadius))
                * (1.0f - _slowestVelocityRatio) + _slowestVelocityRatio;

        return output;
    }

    public bool TargetReached(MovementAgent agent)
    {
        return (agent.transform.position - _target).sqrMagnitude <= _stopRadius * _stopRadius;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(_target, Vector3.up, _stopRadius);

        if (_slowdownRadius > 0.0f)
        {
            Handles.color = new Color(1.0f, 0.5f, 0.0f);
            Handles.DrawWireDisc(_target, Vector3.up, _slowdownRadius);
        }
    }
#endif
}
