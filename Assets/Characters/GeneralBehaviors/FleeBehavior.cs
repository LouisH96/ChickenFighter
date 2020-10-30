using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(FaceBehavior))]
public class FleeBehavior : MonoBehaviour
{
    //---Components---
    private MovementBehavior _movementBehavior = null;
    private FaceBehavior _faceBehavior = null;

    //---Variables---
    private Vector3 _target = Vector3.zero;
    [SerializeField] private Transform _lockTarget = null;
    public Vector3 Target { get { return _target; } set { _target = value; } }
    public Transform LockTarget { get { return _lockTarget; } set { _lockTarget = value; } }

    void Awake()
    {
        _movementBehavior = GetComponent<MovementBehavior>();
        _faceBehavior = GetComponent<FaceBehavior>();
    }

    void Update()
    {
        if (_lockTarget)
            _target = _lockTarget.position;

        HandleRotation();
        HandleMovement();
    }

    private void HandleRotation()
    {
        //set face target as transform + opposite direction to target
        _faceBehavior.Target = transform.position + (transform.position - _target);
    }

    private void HandleMovement()
    {
        float moveRatio = 1.0f;

        //slow based on rotation (0° -> full speed, 180° -> no speed)
        moveRatio *= 1.0f - Mathf.Abs(_faceBehavior.AngleRatio);

        //set move-ratio
        _movementBehavior.DesiredVelocityRatio = moveRatio;
    }
}