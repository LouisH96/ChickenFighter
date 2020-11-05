using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MySessionUtils : MonoBehaviour
{
    private static MySessionUtils _instance = null;
    public static MySessionUtils Instance { get { return _instance; } }

    [SerializeField] private float _timeScale = 1.0f;
    private Plane _cursorMovementPlane;
    private Vector3 _mouseInWorld = Vector3.zero;
    public Vector3 MouseInWorld { get { return _mouseInWorld; } }


    void Awake()
    {
        _instance = this;

        _cursorMovementPlane = new Plane(Vector3.up, 0.0f);
    }

    void Update()
    {
        _mouseInWorld = GetWorldMousePosition();
        Time.timeScale = _timeScale;
    }

    private void OnDrawGizmos()
    {
        if (GizmoManager.Instance &&
            GizmoManager.Instance.DrawMouseInWorld)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_mouseInWorld, 0.05f);
        }
    }

    public Vector3 GetWorldMousePosition()
    {
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

        return positionOfMouseInWorld;
    }
}