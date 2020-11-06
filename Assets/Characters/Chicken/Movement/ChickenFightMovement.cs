using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenFightMovement : MonoBehaviour
{
    //---Components---
    [SerializeField] private Chicken _chicken = null;
    [SerializeField] private MovementAgent _agent = null;

    [SerializeField] private MovementBehavior _retreatBehavior = null;
    [SerializeField] private MovementBehavior _attackBehavior = null;

    [SerializeField] private FleeBehavior _fleeBehavior = null;
    [SerializeField] private SeekBehavior _seekBehavior = null;

    //---Variables---
    [SerializeField] private float _retreatDuration = 4.0f;
    [SerializeField] private float _attackDuration = 5.0f;
    [SerializeField] private float _maxDurationOffRatio = 0.5f; //desired time can be off with x percentage of normal duration
    private float _behaviorTimeLeft = 0.0f;

    void Update()
    {
        _behaviorTimeLeft -= Time.deltaTime;
        if (_behaviorTimeLeft <= 0.0f)
            SwapBehavior();
    }

    private void SwapBehavior()
    {
        if (_agent.MovementBehavior == _retreatBehavior)
            SetAttackBehavior();
        else
            SetRetreatBehavior();
    }

    private void OnEnable()
    {
        Chicken enemy = _chicken.BattleEnemy;
        if (!enemy)
            return;

        _fleeBehavior.LockedTarget = enemy.transform;
        _seekBehavior.LockedTarget = enemy.transform;

        if (Random.value < 0.5f)
            SetRetreatBehavior();
        else
            SetAttackBehavior();
    }

    private void OnDisable()
    {
        _fleeBehavior.LockedTarget = null;
        _seekBehavior.LockedTarget = null;

        _agent.enabled = false;
        _retreatBehavior.enabled = false;
        _attackBehavior.enabled = false;
    }

    private void SetRetreatBehavior()
    {
        _agent.enabled = true;

        if (_agent.MovementBehavior)
            _agent.MovementBehavior.enabled = false;

        _agent.MovementBehavior = _retreatBehavior;
        _retreatBehavior.enabled = true;

        _behaviorTimeLeft = _retreatDuration * (1.0f + Random.Range(_maxDurationOffRatio, _maxDurationOffRatio));
    }

    private void SetAttackBehavior()
    {
        _agent.enabled = true;

        if (_agent.MovementBehavior)
            _agent.MovementBehavior.enabled = false;

        _agent.MovementBehavior = _attackBehavior;
        _attackBehavior.enabled = true;

        _behaviorTimeLeft = _attackDuration * (1.0f + Random.Range(_maxDurationOffRatio, _maxDurationOffRatio));
    }
}
