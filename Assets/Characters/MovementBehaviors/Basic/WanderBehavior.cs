using Assets.Characters.MovementBehaviors;
using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WanderBehavior : SeekBehavior
{
    //---Stats---
    [SerializeField] private float _offset = 1.0f;
    [SerializeField] private float _radius = 1.0f;
    [SerializeField] private float _maxChange = MyUtils.ToRadians(10.0f); //in radians

    //---Variables---
    private float _currentAngle = 0.0f; //in radians

    public override MovementOutput HandleMovement(MovementAgent agent)
    {
        ChangeAngle();

        _target = agent.transform.position;
        _target += agent.transform.forward * _offset;
        _target += MyUtils.DegreesToVectorXZ(_currentAngle) * _radius;

        return base.HandleMovement(agent);
    }

    private void ChangeAngle()
    {
        _currentAngle += Random.Range(-_maxChange, _maxChange);
        _currentAngle = MyUtils.ClampRadians(_currentAngle);
    }


    protected override void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        base.OnDrawGizmosSelected();

        Handles.color = Color.white;
        Handles.DrawWireDisc(transform.position + transform.forward * _offset, Vector3.up, _radius);
#endif
    }
}
