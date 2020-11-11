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

    private void Update()
    {
        ChangeAngle();
    }

    public override MovementOutput HandleMovement(MovementAgent agent)
    {
        Target3D = agent.transform.position;
        Target3D += agent.transform.forward * _offset;
        Target3D += MyUtils.DegreesToVectorXZ(_currentAngle) * _radius;

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
        if (!ShowDebugInfo)
            return;

        base.OnDrawGizmosSelected();

        Handles.color = Color.white;
        Handles.DrawWireDisc(transform.position + transform.forward * _offset, Vector3.up, _radius);
        Handles.DrawSolidDisc(Target3D, Vector3.up, 0.05f);
#endif
    }
}
