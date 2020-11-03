using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WallAvoidance : MonoBehaviour
{
    //---Stats---
    [SerializeField] private float _raycastInterval = 0.3f;
    [SerializeField] private float _wallDetectionRadius = 2.0f;
    [SerializeField] private float _wallAvoidanceDist = 0.25f;

    //---Variables---
    private float _raycastTimer = 0.0f;
    private RaycastHit? _wallHit = null;
    private Vector3 _destination = Vector3.zero;

    private Vector3 _wa = Vector3.zero;
    private Vector3 _ha = Vector3.zero;

    private Vector3 _toWall = Vector3.zero;

    void Start()
    {
    }

    void Update()
    {
        _raycastTimer += Time.deltaTime;
        if (_raycastTimer >= _raycastInterval)
        {
            DoRaycast();
            _raycastTimer = 0.0f;
        }

        if (_wallHit != null)
        {
            Vector3 wallHit = ((RaycastHit)_wallHit).point;
            Vector3 wallAvoidancePoint = wallHit + ((RaycastHit)_wallHit).normal * _wallAvoidanceDist;

            Debug.DrawLine(transform.position, wallHit);
            Debug.DrawLine(wallHit, wallAvoidancePoint);
        }
        else
            Debug.DrawLine(transform.position, transform.position + transform.forward * _wallDetectionRadius);

    }

    void DoRaycast()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, _wallDetectionRadius, LayerMask.GetMask("StaticLevel", "DynamicLevel")))
            _wallHit = hit;
        else
        {
            _wallHit = null;
            return;
        }

        Vector3 uw = -hit.normal; //unitvector to wall
        uw.y = 0.0f;
        Vector3 uh = transform.forward; //unitvector to hit
        uh.y = 0.0f;
        float coshw = Vector3.Dot(uh, uw); //cos between towall and tohit

        Vector3 w = uw * hit.distance * coshw; //to wall
        Vector3 wa = w + hit.normal * _wallAvoidanceDist;  //towall - avoidance

        _toWall = transform.position + w;

        float waToUnitLength = wa.x / (-hit.normal.x * _wallDetectionRadius);

        float sinG = waToUnitLength; // sin of vector to goal
        float cosG = Mathf.Cos(Mathf.Asin(sinG));

        Vector3 ha = hit.point - transform.position + hit.normal * _wallAvoidanceDist; //to hitposition - wallavoidance

        _wa = wa + transform.position;
        _ha = ha + transform.position;

        Vector3 waToHa = (ha - wa);
        float waToHaLength = Mathf.Sin(Mathf.Acos(coshw)) * hit.distance;

        Vector3 waToG = waToHa * ((cosG * _wallDetectionRadius) / waToHaLength);
        _destination = transform.position + wa + waToG;

        Debug.DrawLine(transform.position + wa, transform.position + wa + waToHa, Color.white, 0.31f);
    }

    private void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        Handles.color = Color.white;
        Handles.DrawWireDisc(transform.position, Vector3.up, _wallDetectionRadius);

        if (_wallHit != null)
        {
            Gizmos.DrawSphere(_toWall, 0.1f);
            Debug.DrawLine(_toWall, _toWall + ((RaycastHit)_wallHit).normal * Vector3.Distance(transform.position, _toWall));
            Gizmos.DrawSphere(_destination, 0.1f);
            Gizmos.DrawSphere(_ha, 0.1f);
            Gizmos.DrawSphere(_wa, 0.1f);
        }
#endif
    }
}
