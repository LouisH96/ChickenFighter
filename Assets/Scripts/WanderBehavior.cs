using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WanderBehavior : MonoBehaviour
{
    //---Components---
    private MovementBehavior _movementBehavior = null;

    //---Stats---
    [SerializeField] private float _maxRatioChange = 0.5f;
    private float _currentRatio = 0.0f;


    private void Awake()
    {
        _movementBehavior = GetComponent<MovementBehavior>();
    }

    //private void Update()
    //{
    //    if (!_movementBehavior)
    //        return;

    //    _currentRatio += Random.Range(-_maxRatioChange, _maxRatioChange);
    //    _currentRatio = Mathf.Clamp(_currentRatio, -1.0f, 1.0f);

    //    _movementBehavior.RotationRatio = _currentRatio;
    //    _movementBehavior.ForwardMovementRatio = 1.0f;
    //}

    //wander based on circle in front of character
    [SerializeField] private float _offset = 1.0f;
    [SerializeField] private float _radius = 1.0f;
    [SerializeField] private float _maxChange = MyUtils.ToRadians(10.0f); //in radians
    private float _currentAngle = 0.0f; //in radians

    void Update()
    {
        if (_movementBehavior == null)
            return;

        ChangeAngle();

        //  Vector3 circlePos = transform.position + transform.forward * _offset;
        //Vector3 wanderTarget = circlePos + MyUtils.DegreesToVectorXZ(_currentAngle) * _radius;

        Vector3 t = MyUtils.DegreesToVectorXZ(_currentAngle);

        if ((t).x > 0.0f)
            _movementBehavior.RotationRatio = 1.0f;
        else
            _movementBehavior.RotationRatio = -1.0f;

        _movementBehavior.ForwardMovementRatio = 1.0f;
        //_movementBehavior.Destination = wanderTarget;
    }

    void ChangeAngle()
    {
        _currentAngle += Random.Range(-_maxChange, _maxChange);
        _currentAngle = MyUtils.ClampRadians(_currentAngle);
    }


    //private PositionMovementBehavior _movementBehavior;

    ////wander based on circle in front of character
    //[SerializeField] private float _offset = 1.0f;
    //[SerializeField] private float _radius = 1.0f;
    //[SerializeField] private float _maxChange = MyUtils.ToRadians(10.0f); //in radians
    //private float _currentAngle = 0.0f; //in radians

    //private void Awake()
    //{
    //    _movementBehavior = GetComponent<PositionMovementBehavior>();
    //}

    //void Update()
    //{
    //    if (_movementBehavior == null)
    //        return;

    //    ChangeAngle();

    //    Vector3 circlePos = transform.position + transform.forward * _offset;
    //    Vector3 wanderTarget = circlePos + MyUtils.DegreesToVectorXZ(_currentAngle) * _radius;

    //    _movementBehavior.Destination = wanderTarget;
    //}

    //void ChangeAngle()
    //{
    //    _currentAngle += Random.Range(-_maxChange, _maxChange);
    //    _currentAngle = MyUtils.ClampRadians(_currentAngle);
    //}
}
