using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(FaceBehavior))]
public class SeekBehavior : MonoBehaviour
{
    //---Components---
    private MovementBehavior _movementBehavior = null;
    private FaceBehavior _faceBehavior = null;

    //---Stats---
    [SerializeField] private float _stopRadius = 0.25f;
    [SerializeField] private float _slowDownRadius = 1.0f;

    //---Variables---
    private Vector3 _target = Vector3.zero;
    public Vector3 Target { set { _target = value; } get { return _target; } }

    [SerializeField] private Transform _lockTarget = null;
    public Transform LockTarget { set { _lockTarget = value; } get { return _lockTarget; } }

    private bool _targetReached = false;
    public bool TargetReached { get { return _targetReached; } }



    void Awake()
    {
        _movementBehavior = GetComponent<MovementBehavior>();
        _faceBehavior = GetComponent<FaceBehavior>();
        _target = transform.position;
    }

    void Update()
    {
        if (_lockTarget)
            _target = _lockTarget.position;

        HandleRotation();
        HandleMovement();
    }

    void HandleRotation()
    {
        _faceBehavior.Target = _target;
    }

    void HandleMovement()
    {
        float distanceToTargetSqr = Vector3.SqrMagnitude(_target - transform.position);
        
        //stop if within stop-radius
        if (distanceToTargetSqr < _stopRadius * _stopRadius)
        {
            _movementBehavior.DesiredVelocityRatio = 0.0f;
            _targetReached = true;
            return;
        }

        _targetReached = false;
        float moveRatio = 1.0f;
        
        //slow if within slow-radius
        float sqrSlowdownRadius = _slowDownRadius * _slowDownRadius;
        if (sqrSlowdownRadius > 0.0f && distanceToTargetSqr < sqrSlowdownRadius)
            moveRatio *= distanceToTargetSqr / sqrSlowdownRadius;

        //slow based on rotation (0° -> full speed, 180° -> no speed)
        moveRatio *= 1.0f - Mathf.Abs(_faceBehavior.AngleRatio);

        //set move-ratio
        _movementBehavior.DesiredVelocityRatio = moveRatio;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(_target, Vector3.up, _stopRadius);

        Handles.color = new Color(1.0f, 0.5f, 0.0f);
        Handles.DrawWireDisc(_target, Vector3.up, _slowDownRadius);
    }
#endif
}