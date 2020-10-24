using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _jumpForce = 5.0f;

    private PositionMovementBehavior _movementBehavior;
    private Rigidbody _rigidBody = null;

    void Awake()
    {
        _movementBehavior = GetComponent<PositionMovementBehavior>();
        _rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (_movementBehavior == null)
            return;

        float horizontalMovement = Input.GetAxis("MovementHorizontal");
        float verticalMovement = Input.GetAxis("MovementVertical");

        Vector3 movement = horizontalMovement * Vector3.right + verticalMovement * Vector3.forward;
        movement = transform.rotation * movement;

        _movementBehavior.SetDestinationByDirection(movement);

        
    }

    void FixedUpdate()
    {
        if (Input.GetAxis("Jump") > 0.0f && Mathf.Approximately(_rigidBody.velocity.y, Mathf.Epsilon))
        {
            _rigidBody.AddForce(transform.up * _jumpForce);
        }
    }
}
