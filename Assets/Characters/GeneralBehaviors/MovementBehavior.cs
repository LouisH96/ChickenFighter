using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovementBehavior : MonoBehaviour
{
    //---Components---
    private CharacterController _characterController = null;

    //Stats
    [SerializeField] private float _forwardAcceleration = 6.0f;
    [SerializeField] private float _maxFowardVelocity = 3.0f;
    [SerializeField] private float _rotateSpeed = 60.0f;
    [SerializeField] private float _jumpForce = 5.0f;
    private float _gravity = -9.81f;

    //public
    private float _desiredVelocity = 0.0f;
    public float DesiredVelocityRatio { set { _desiredVelocity = value * _maxFowardVelocity; } }

    private float _rotation = 0.0f;
    public float RotationRatio { set { _rotation = value * _rotateSpeed; } get { return _rotation / _rotateSpeed; } }

    private bool _shouldJump = false;
    public bool ShouldJump { set { _shouldJump = value; } }

    //variables
    private Vector3 _currentVelocity = Vector3.zero;


    void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    void FixedUpdate()
    {
        HandleRotation();
        HandleMovement();
    }

    private void HandleRotation()
    {
        transform.Rotate(0, _rotation * Time.fixedDeltaTime, 0);
    }

    private void HandleMovement()
    {


        ////forward movement
        //if (_acceleration != 0.0f)
        //{
        //    _currentVelocity.z += _acceleration * Time.fixedDeltaTime;
        //    _currentVelocity.z = Mathf.Clamp(_currentVelocity.z, -_maxFowardVelocity, _maxFowardVelocity);
        //}
        //else

        //move
        _currentVelocity.z = Mathf.MoveTowards(_currentVelocity.z, _desiredVelocity, _forwardAcceleration * Time.fixedDeltaTime);

        //jump
        if (_shouldJump && _characterController.isGrounded)
            _currentVelocity.y += _jumpForce;

        //gravity
        _currentVelocity.y += _gravity * Time.fixedDeltaTime;

        //do movement
        _characterController.Move(transform.TransformDirection(_currentVelocity) * Time.fixedDeltaTime);

        //ground collision
        if (_characterController.isGrounded)
            _currentVelocity.y = 0.0f;
    }
}