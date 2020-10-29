using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Transform _target = null;
    [SerializeField]
    private float _followSpeed = 5.0f;
    [SerializeField]
    private float _rotateSpeed = 5.0f;

    private void Update()
    {
        HandleRotation();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        if (!_target)
            return;

        Vector3 targetPosition = _target.position;
        targetPosition.y = transform.position.y;

        transform.position = Vector3.Lerp(transform.position,
            targetPosition, _followSpeed * Time.deltaTime);
    }

    void HandleRotation()
    {
        if (!_target)
            return;

        transform.rotation = Quaternion.Slerp(transform.rotation, _target.rotation, _rotateSpeed * Time.deltaTime);
    }
}
