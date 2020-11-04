using Assets.Characters.MovementBehaviors;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovementAgent : MonoBehaviour
{
    //---Components---
    private CharacterController _characterController;
    [SerializeField] private MovementBehavior2 _movementBehavior;
    [SerializeField] private RotationBehavior _rotationBehavior;

    //---Stats---
    //movement
    [SerializeField] private float _acceleration = 6.0f;
    [SerializeField] private float _deacceleration = 6.0f;
    [SerializeField] private float _maxVelocity = 3.0f;
    //jump
    [SerializeField] private float _jumpForce = 5.0f;
    [SerializeField] private float _gravity = -9.81f;
    //rotate
    [SerializeField] private float _maxAngularVelocity = 60.0f;

    //---Variables---
    private Vector3 _velocity = Vector3.zero;
    private float _angularVelocity = 0.0f;
    private MovementOutput _movementOutput = new MovementOutput { IsValid = false, DesiredForwardVelocity = 0.0f, ShouldJump = false };
    private RotationOutput _rotationOutput = new RotationOutput { IsValid = false, DesiredAngularVelocity = 0.0f };

    //---Public---
    public CharacterController CharacterController { get { return _characterController; } }
    public float Acceleration { get { return _acceleration; } set { _acceleration = value; } }
    public float Deacceleration { get { return _deacceleration; } set { _deacceleration = value; } }
    public float MaxVelocity { get { return _maxVelocity; } set { _maxVelocity = value; } }
    public float JumpForce { get { return _jumpForce; } set { _jumpForce = value; ; } }
    public float MaxAngularVelocity { get { return _maxAngularVelocity; } set { _maxAngularVelocity = value; } }
    public Vector3 Velocity { get { return _velocity; } set { _velocity = value; } }
    public float AngularVelocity { get { return _angularVelocity; } set { _angularVelocity = value; } }

    //---Functions---
    void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        if (!_movementBehavior)
            _movementBehavior = GetComponent<MovementBehavior2>();

        if (!_rotationBehavior)
            _rotationBehavior = GetComponent<RotationBehavior>();
    }

    void Update()
    {
        if (_movementBehavior)
            _movementOutput = _movementBehavior.HandleMovement(this);

        if (_rotationBehavior)
            _rotationOutput = _rotationBehavior.HandleRotation(this);
    }

    private void FixedUpdate()
    {
        HandleMovement(_movementOutput);
        HandleRotation(_rotationOutput);
    }

    private void HandleMovement(MovementOutput output)
    {
        if (!output.IsValid || !_movementBehavior)
            return;

        //move
        _velocity.z = Mathf.MoveTowards(_velocity.z, output.DesiredForwardVelocity, _acceleration * Time.fixedDeltaTime);

        //jump
        if (output.ShouldJump && _characterController.isGrounded)
            _velocity.y += _jumpForce;

        //gravity
        _velocity.y += _gravity * Time.fixedDeltaTime;

        //do movement
        _characterController.Move(transform.TransformDirection(_velocity) * Time.fixedDeltaTime);

        //ground collision
        if (_characterController.isGrounded)
            _velocity.y = 0.0f;
    }

    private void HandleRotation(RotationOutput output)
    {
        if (!output.IsValid || !_rotationBehavior)
            return;

        //limit angular velocity
        float angVel = output.DesiredAngularVelocity;
        if (angVel > MaxAngularVelocity)
            angVel = MaxAngularVelocity;

        transform.Rotate(0.0f, angVel * Time.fixedDeltaTime, 0.0f);
    }
}