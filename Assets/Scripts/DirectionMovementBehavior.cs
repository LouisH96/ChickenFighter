using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionMovementBehavior : MonoBehaviour
{
    //---Private---
    [SerializeField]
    private float _movementSpeed = 1.0f;
    [SerializeField]
    private float _rotationSpeed = 1.0f;

    private Vector3 _desiredMovementDirection = Vector3.zero;
    private Rigidbody _rigidBody = null;

    //---Public---
    public Vector3 DesiredMovementDirection
    {
        get { return _desiredMovementDirection; }
        set { _desiredMovementDirection = value.normalized; }
    }

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandleRotation();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        Vector3 currentPos = transform.position;
        Vector3 movement = _desiredMovementDirection * _movementSpeed * Time.fixedDeltaTime;

        _rigidBody.MovePosition(_rigidBody.position + movement);
    }

    void HandleRotation()
    {
        transform.LookAt(transform.position + transform.forward, Vector3.up);

        //newPos.y = transform.position.y;
        //if (newPos != currentPos)
        //{
        //    Quaternion currentRot = transform.rotation;
        //    Quaternion desiredRot = Quaternion.LookRotation(newPos - transform.position);
        //    float maxRot = _rotationSpeed * Time.fixedDeltaTime;

        //    transform.rotation = Quaternion.Slerp(currentRot, desiredRot, maxRot);
        //}
    }
}
