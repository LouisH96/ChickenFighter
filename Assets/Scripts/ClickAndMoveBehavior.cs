using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickAndMoveBehavior : MonoBehaviour
{
    private PositionMovementBehavior _movementBehavior;
    private Plane _cursorMovementPlane;

    void Awake()
    {
        _movementBehavior = GetComponent<PositionMovementBehavior>();
        _cursorMovementPlane = new Plane(Vector3.up, transform.position);
    }

    void Update()
    {
        HandleMovementInput();
    }

    private void HandleMovementInput()
    {
        if (_movementBehavior == null)
            return;

        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        Vector3 positionOfMouseInWorld = transform.position;

        RaycastHit hitInfo;
        if (Physics.Raycast(mouseRay, out hitInfo, 1000000.0f,
            LayerMask.GetMask("Ground")))
        {
            positionOfMouseInWorld = hitInfo.point;
        }
        else
        {
            _cursorMovementPlane.Raycast(mouseRay, out float distance);
            positionOfMouseInWorld = mouseRay.GetPoint(distance);
        }

        _movementBehavior.Destination =  positionOfMouseInWorld;
    }
}
