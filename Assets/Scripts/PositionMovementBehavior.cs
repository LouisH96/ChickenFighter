using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionMovementBehavior : MonoBehaviour
{
    //---Private---
    [SerializeField]
    private float _movementSpeed = 1.0f;
    [SerializeField]
    private float _rotationSpeed = 1.0f;

    private Vector3 _destination = Vector3.zero;
    private Rigidbody _rigidBody = null;

    //---Public---
    public Vector3 Destination
    {
        get { return _destination; }
        set { _destination = value; }
    }

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        //movement
        Vector3 currentPos = transform.position;
        float maxDistance = _movementSpeed * Time.fixedDeltaTime;

        Vector3 newPos
            = Vector3.MoveTowards(currentPos, _destination, maxDistance);

        _rigidBody.MovePosition(newPos);

        //rotation
        newPos.y = transform.position.y;
        if (newPos != currentPos)
        {
            Quaternion currentRot = transform.rotation;
            Quaternion desiredRot = Quaternion.LookRotation(newPos - transform.position);
            float maxRot = _rotationSpeed * Time.fixedDeltaTime;

            transform.rotation = Quaternion.Slerp(currentRot, desiredRot, maxRot);
        }
    }

    public void SetDestinationByDirection(Vector3 normalizedDirection)
    {
        _destination = transform.position +  normalizedDirection * _movementSpeed;
    }
}
