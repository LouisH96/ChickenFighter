using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WanderBehavior : MonoBehaviour
{
    private PositionMovementBehavior _movementBehavior;

    //wander based on circle in front of character
    [SerializeField] private float _offset = 1.0f;
    [SerializeField] private float _radius = 1.0f;
    [SerializeField] private float _maxChange = MyUtils.ToRadians(10.0f); //in radians
    private float _currentAngle = 0.0f; //in radians

    private void Awake()
    {
        _movementBehavior = GetComponent<PositionMovementBehavior>();
    }

    void Update()
    {
        if (_movementBehavior == null)
            return;

        ChangeAngle();

        Vector3 circlePos = transform.position + transform.forward * _offset;
        Vector3 wanderTarget = circlePos + MyUtils.DegreesToVectorXZ(_currentAngle) * _radius;
        
        _movementBehavior.Destination = wanderTarget;
    }

    //private void OnDrawGizmos()
    //{
    //    if (!GizmoManager.Instance.DrawWanderCircle)
    //        return;

    //    Vector3 circlePos = transform.position + transform.forward * _offset;
    //    Vector3 wanderTarget = circlePos + MyUtils.DegreesToVectorXZ(_currentAngle) * _radius;

    //    Handles.color = Color.white;
    //    Handles.DrawWireDisc(circlePos, Vector3.up, _radius);

    //    Handles.DrawWireDisc(wanderTarget, Vector3.up, 0.2f);
    //}

    void ChangeAngle()
    {
        _currentAngle += Random.Range(-_maxChange, _maxChange);
        _currentAngle = MyUtils.ClampRadians(_currentAngle);
    }
}
