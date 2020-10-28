using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //---Components---
    private MovementBehavior _movementBehavior = null;

    void Awake()
    {
        _movementBehavior = GetComponent<MovementBehavior>();
    }

    void Update()
    {
        if (_movementBehavior == null)
            return;

        //set inputs
        _movementBehavior.ForwardMovementRatio = Input.GetAxis("MovementVertical");
        _movementBehavior.RotationRatio = Input.GetAxis("MovementHorizontal");
        _movementBehavior.ShouldJump = Input.GetAxis("Jump") > 0.0f;
    }
}