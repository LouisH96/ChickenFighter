using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

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

    private Vector3 _debugToHitPos = Vector3.zero;
    private Vector3 _debugToHitAvoidance = Vector3.zero;
    private Vector3 _debugToGoal = Vector3.zero;

    void Start()
    {
    }

    void Update()
    {
        _raycastTimer += Time.deltaTime;
        if (_raycastTimer >= _raycastInterval)
        {
            FindEscapeDestination();
            DoCornerCheck();
            _raycastTimer = 0.0f;
        }
    }

    private void DoCornerCheck()
    {
    }

    void FindEscapeDestination()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, _wallDetectionRadius, LayerMask.GetMask("StaticLevel", "DynamicLevel")))
        {
            _wallHit = hit;
        }
        else
        {
            _wallHit = null;
            return;
        }

        //if collide with wall
        //calculate point that is _wallAvoidanceDistance in front of the hitPoint, along the hit.normal
        //move it sideways parallel along the wall so it touches the _walldetectionradius of the player
        //(will be (abit)wrong when hit & player y is not equal (cuz hit.distance))
        //(doesn't work perfectly on round walls/surfaces, destination is not on _walldetectionradius)
        //(doesn't work either when player is (almost) perpendicular to wall)
        Vector2 wallNormal = new Vector2(hit.normal.x, hit.normal.z);
        Vector2 toWallUnit = -wallNormal;
       
        Vector2 toHitUnit = new Vector2(transform.forward.x, transform.forward.z);
        float cosHitAndWall = Vector2.Dot(toHitUnit, toWallUnit); //cos between the vector to the hit and the vector perpendicular to the wall

        float toWallDistance = hit.distance * cosHitAndWall;
        Vector2 toWall = toWallUnit * toWallDistance; //vector from player to wall (perpendicular). (=projection toHit on toWallUnit)
        Vector2 toWallAvoidance = toWall - toWallUnit * _wallAvoidanceDist; //toWall - _wallAvoidanceDist

        float goalSin = (toWallDistance - _wallAvoidanceDist) / _wallDetectionRadius; //unit-sin of the goal point
        float goalCos = Mathf.Cos(Mathf.Asin(goalSin)); //unit-sin of the goal point

        Vector2 playerPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 hitPos = new Vector2(hit.point.x, hit.point.z);
        Vector2 toHit = hitPos - playerPos; //playerpos to hitpos
        
        Vector2 wallToHit = toHit - toWall; //vector from toWall to hitPoint (= from toWallAvoidance to toHitAvoidance, =a vector (not unit) in the direction to the goal) 
        float wallToHitLength = Mathf.Sin(Mathf.Acos(cosHitAndWall)) * hit.distance;

        //fix if player is perpendicular to wall
        if (wallToHitLength == 0.0f || float.IsNaN(wallToHitLength))
        {
            wallToHitLength = 1.0f;
            wallToHit = Vector2.Perpendicular(toWallUnit);
        }

        float wallAvoidanceToGoalLength = goalCos * _wallDetectionRadius;
        Vector2 wallAvoidanceToGoal = wallToHit * (wallAvoidanceToGoalLength / wallToHitLength);

        Vector2 toGoal2D = toWallAvoidance + wallAvoidanceToGoal;
        Vector3 toGoal3D = new Vector3(toGoal2D.x, 0.0f, toGoal2D.y);

        _destination = transform.position + toGoal3D;

        //set debug points
        Vector2 toHitAvoidance = toWallAvoidance + wallToHit;
        _debugToHitPos = new Vector3(toHit.x, transform.position.y, toHit.y);
        _debugToHitAvoidance = new Vector3(toHitAvoidance.x, transform.position.y, toHitAvoidance.y);
        _debugToGoal = new Vector3(toGoal2D.x, transform.position.y, toGoal2D.y);
     }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.white;
        Handles.DrawWireDisc(transform.position, Vector3.up, _wallDetectionRadius);

        if (_wallHit != null)
        {
            //draw 'path' from player to goal
            Vector3 start = transform.position;
            Vector3 end = transform.position + _debugToHitPos;
            Debug.DrawLine(start, end);

            start = end;
            end = transform.position + _debugToHitAvoidance;
            Debug.DrawLine(start, end);

            start = end;
            end = transform.position + _debugToGoal;
            Debug.DrawLine(start, end);
        }
        else
        {
            //draw detectionRay
            Debug.DrawLine(transform.position, transform.position + transform.forward * _wallDetectionRadius);
        }
    }
#endif
}
