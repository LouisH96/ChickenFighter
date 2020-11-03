using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshFleeBehavior : MonoBehaviour
{
    //---Components---
    private NavMeshAgent _navMeshAgent = null;

    //---Stats---
    [SerializeField] private float _fleeDistance = 4.0f;

    //---Variables--
    private Transform _fleeTarget = null;

    //---Get/set---
    public Transform FleeTarget
    {
        get { return _fleeTarget; }
        set { _fleeTarget = value; }
    }

    void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        if (_navMeshAgent && _fleeTarget)
            FindNewPath();
    }

    void Update()
    {
        if (HasDestinationReached()
            && Vector3.SqrMagnitude(_fleeTarget.position - transform.position)
            < Mathf.Pow( _fleeDistance + _navMeshAgent.stoppingDistance *2.0f + 0.1f, 2.0f))
            FindNewPath();
    }

    private void FindNewPath()
    {
        if (!_navMeshAgent || !_fleeTarget)
            return;

        Vector3 newDestination;
        NavMeshPath path = new NavMeshPath();
        NavMeshHit hit;

        //first try to flee in opposite direction
        newDestination = transform.position + (transform.position - _fleeTarget.position).normalized * _fleeDistance;
        if (_navMeshAgent.CalculatePath(newDestination, path))
        {
            _navMeshAgent.destination = newDestination;
            return;
        }

        //if blocked, sample closest position
        else if (NavMesh.SamplePosition(newDestination, out hit, _fleeDistance * 2.0f, 0))
        {
            newDestination = hit.position;
            if (_navMeshAgent.CalculatePath(newDestination, path)
                  && Vector3.SqrMagnitude(_fleeTarget.position - newDestination) >= _fleeDistance * _fleeDistance)
            {
                _navMeshAgent.destination = newDestination;
                return;
            }
        }

        //try x-times to get random destination
        int maxAttempts =50;

        for (int i = 0; i < maxAttempts; i++)
        {
            newDestination = Random.insideUnitCircle.normalized * _fleeDistance;
            newDestination += _fleeTarget.position;

            if (!NavMesh.SamplePosition(newDestination, out hit, _fleeDistance * 1.1f, -1))
                continue;

            newDestination = hit.position;

            if (!_navMeshAgent.CalculatePath(newDestination, path))
                continue;

            //if (Vector3.SqrMagnitude(_fleeTarget.position - newDestination) < _fleeDistance * _fleeDistance)
            //    continue;

            _navMeshAgent.destination = newDestination;
            return;
        }
    }

    private bool HasDestinationReached()
    {
        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            if (!_navMeshAgent.hasPath || Mathf.Abs(_navMeshAgent.velocity.sqrMagnitude) < float.Epsilon)
                return true;
        }
        return false;
    }
}