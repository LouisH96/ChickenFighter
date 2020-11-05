using Assets.Characters.MovementBehaviors;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

public class WallAvoidanceBehavior : ArriveBehavior
{
    //---Stats---
    [SerializeField] private float _raycastInterval = 0.3f;
    [SerializeField] private float _wallDetectionRadius = 2.0f;
    [SerializeField] private float _wallAvoidanceDist = 0.25f;

    //---Variables---
    private float _raycastTimer = 0.0f;
    private RaycastHit? _wallHit = null;

    private Vector3 _debugToHitPos = Vector3.zero;
    private Vector3 _debugToHitAvoidance = Vector3.zero;
    private Vector3 _debugToGoal = Vector3.zero;

    private List<Vector3> _debugCornerChecks = new List<Vector3>();

    private bool _isAvoidingWall = false;
    private bool _recast = false;

    //---Public---
    public bool HasWallHit { get { return _wallHit != null; } }

    void Start()
    {
    }

    public override MovementOutput HandleMovement(MovementAgent agent)
    {
        if (_isAvoidingWall)
        {
            MovementOutput output = base.HandleMovement(agent);
            _isAvoidingWall = output.IsValid;
            return output;
        }
        else
        {
            if (_recast)
            {
                CastWallDetectionRay();
                FindEscapeDestination();
                DoCornerCheck();
                _recast = false;
            }

            if (!HasWallHit)
                return new MovementOutput { IsValid = false };
            else
            {
                FindEscapeDestination();
                DoCornerCheck();
                _isAvoidingWall = true;
                return this.HandleMovement(agent);
            }
        }
    }

    void Update()
    {
        _raycastTimer += Time.deltaTime;
        if (_raycastTimer >= _raycastInterval)
        {
            _recast = true; ;
            _raycastTimer = 0.0f;
        }
    }


    private void CastWallDetectionRay()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, _wallDetectionRadius, LayerMask.GetMask("StaticLevel", "DynamicLevel")))
            _wallHit = hit;
        else
            _wallHit = null;
    }

    private void DoCornerCheck()
    {
        if (_wallHit == null)
            return;

        _debugCornerChecks.Clear();
        int checks = 8;

        float randomSign = UnityEngine.Random.value < 0.5 ? -1.0f : 1.0f;

        Vector3 rayVector = Target - transform.position;
        rayVector = rayVector.normalized * _wallDetectionRadius;
        Ray ray = new Ray(transform.position, rayVector);
        Quaternion rotation = Quaternion.Euler(0.0f, randomSign * 360.0f / checks, 0.0f);

        for (int i = 0; i < checks; i++)
        {
            _debugCornerChecks.Add(transform.position);
            _debugCornerChecks.Add(transform.position + rayVector);
            if (!Physics.Raycast(ray, out RaycastHit hit, _wallDetectionRadius, LayerMask.GetMask("StaticLevel", "DynamicLevel")))
            {
                Target = rayVector + transform.position;
                return;
            }

            rayVector = rotation * rayVector;
            ray.direction = rayVector;
        }
    }

    void FindEscapeDestination()
    {
        if (_wallHit == null)
            return;

        RaycastHit hit = (RaycastHit)_wallHit;

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

        float distToWallAvoidancePoint = toWallDistance - _wallAvoidanceDist;

        float goalSin = distToWallAvoidancePoint / _wallDetectionRadius; //unit-sin of the goal point
        if (goalSin > 1.0f)
            goalSin = 1.0f;
        else
            if (goalSin < -1.0f)
            goalSin = -1.0f;

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

        Target = transform.position + toGoal3D;

        //set debug points
        Vector2 toHitAvoidance = toWallAvoidance + wallToHit;
        _debugToHitPos = new Vector3(toHit.x, transform.position.y, toHit.y);
        _debugToHitAvoidance = new Vector3(toHitAvoidance.x, transform.position.y, toHitAvoidance.y);
        _debugToGoal = new Vector3(toGoal2D.x, transform.position.y, toGoal2D.y);
    }

    protected override void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        if (!ShowDebugInfo)
            return;

        Handles.color = Color.white;
        Handles.DrawWireDisc(transform.position, Vector3.up, _wallDetectionRadius);

        if (_wallHit != null)
        {
            if (_debugCornerChecks.Count > 2)
            {
                for (int i = 0; i < _debugCornerChecks.Count - 1; i += 2)
                {
                    Color color = i == _debugCornerChecks.Count - 2 ? Color.yellow : Color.white;
                    Debug.DrawLine(_debugCornerChecks[i], _debugCornerChecks[i + 1], color);
                }
            }
            else
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

                Debug.DrawLine(transform.position, Target, Color.green);
            }

            Handles.color = Color.red;
            Handles.DrawWireDisc(Target, Vector3.up, base.StopRadius);
        }
        else
        {
            //draw detectionRay
            Debug.DrawLine(transform.position, transform.position + transform.forward * _wallDetectionRadius);
        }
#endif
    }
}
