using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementBehavior))]
public class FaceBehavior : MonoBehaviour
{
    //---Components---
    private MovementBehavior _movementBehavior = null;

    //---Stats---
    [SerializeField] private float _slowDownAngle = 35.0f;

    //variables
    private Vector3 _target = Vector3.zero;
    public Vector3 Target { set { _target = value; } }

    private float _angleRatio = 0.0f;
    public float AngleRatio { get { return _angleRatio; } }


    void Awake()
    {
        _movementBehavior = GetComponent<MovementBehavior>();
    }

    void Update()
    {
        float angle = Vector3.SignedAngle(transform.forward, _target - transform.position, Vector3.up);
        _angleRatio = angle / 180.0f;

        //no rotation needed if already facing target
        if (angle == 0.0f)
            return;

        float unsignedAngle = Mathf.Abs(angle);
        float sign = Mathf.Sign(angle);

        float rotationSpeedRatio = 1.0f;

        //slow down if close to target
        if (_slowDownAngle > 0.0f && unsignedAngle < _slowDownAngle)
        {
            rotationSpeedRatio = unsignedAngle / _slowDownAngle;
        }

        _movementBehavior.RotationRatio = sign * rotationSpeedRatio;
    }
}