using System.Collections;
using System.Collections.Generic;
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

    void Awake()
    {
        _movementBehavior = GetComponent<MovementBehavior>();
        _faceBehavior = GetComponent<FaceBehavior>();
    }

    void Update()
    {
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
            return;
        }

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
}