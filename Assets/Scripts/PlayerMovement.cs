using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //stats
    [SerializeField] private float _forwardAcceleration = 6.0f;
    [SerializeField] private float _maxFowardVelocity = 3.0f;
    [SerializeField] private float _rotateSpeed = 60.0f;
    [SerializeField] private float _jumpForce = 5.0f;
    private float _gravity = -9.81f;

    //components
    private CharacterController _characterController = null;

    //members
    private Vector3 _currentAcceleration = Vector3.zero;
    private Vector3 _currentVelocity = Vector3.zero;

    void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (_characterController == null)
            return;

        //get input
        float horizontalMovement = Input.GetAxis("MovementHorizontal");
        float verticalMovement = Input.GetAxis("MovementVertical");
        float jumpInput = Input.GetAxis("Jump");

        //set currentAcceleration
        _currentAcceleration.x = horizontalMovement * _rotateSpeed;
        _currentAcceleration.z = verticalMovement * _forwardAcceleration;
        _currentAcceleration.y = jumpInput;
    }

    void FixedUpdate()
    {
        //rotate
        transform.Rotate(0, _currentAcceleration.x * Time.fixedDeltaTime, 0);

        //forward movement
        if (_currentAcceleration.z != 0.0f)
        {
            _currentVelocity.z += _currentAcceleration.z * Time.fixedDeltaTime;
            _currentVelocity.z = Mathf.Clamp(_currentVelocity.z, -_maxFowardVelocity, _maxFowardVelocity);
        }
        else
            _currentVelocity.z = Mathf.MoveTowards(_currentVelocity.z, 0.0f, _forwardAcceleration * Time.fixedDeltaTime);

        //jump
        if (_currentAcceleration.y > 0.0f && _characterController.isGrounded)
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