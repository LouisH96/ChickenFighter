using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class Chicken : MonoBehaviour
{
    public enum ChickenState
    {
        Farm, Fight, PickedUp, Thrown, None
    }

    //---Components---
    [SerializeField] private ChickenStats _stats = null;
    [SerializeField] private ChickenPhysical _physical = null;
    [SerializeField] private ChickenMovement _movement = null;
    [SerializeField] private Renderer _highlightRenderer = null;
    [SerializeField] private AttackZone _attackZone = null;
    [SerializeField] private Animator _animator = null;

    //---Variables---
    [SerializeField] private ChickenState _state = ChickenState.None;
    private ChickenBattle _battle = null;
    private float _currentHealth = 50.0f;

    //---Public---
    public ChickenStats Stats { get { return _stats; } }

    public ChickenBattle Battle { get { return _battle; } }

    public Chicken BattleClosestEnemy
    {
        get
        {
            if (_battle)
                return _battle.GetClosestEnemy(this);
            else
                return null;
        }
    }

    public bool IsAttacking
    {
        get { return _animator.GetBool("Attack"); }
        set { _animator.SetBool("Attack", value); }
    }

    public float CurrentHealthRatio { get { return _currentHealth / _stats.Health; } }
    public float CurrentHealth { get { return _currentHealth; } }
    public float MaxHealth { get { return _stats.Health; } }

    void Awake()
    {
    }

    private void Start()
    {
        ChangeState(_state);
        _currentHealth = _stats.Health;
    }

    public void OnMaxHealthUpgraded(float amount)
    {
        _currentHealth += amount;

        if (_currentHealth > _stats.Health)
            _currentHealth = _stats.Health;
    }
    public void OnMaxSpeedUpgraded()
    {
        _movement.Agent.MaxVelocity = _stats.MaxSpeed;
    }

    public void OnAccelerationUpgraded()
    {
        _movement.Agent.Acceleration = _stats.Acceleration;
    }

    public void ChangeState(ChickenState newState)
    {
        _movement.ChangeState(newState);
        _physical.ChangeState(newState);

        _attackZone.enabled = newState == ChickenState.Fight;
        IsAttacking = false;

        _state = newState;
    }

    public void SetHighlight(bool isHighlighted)
    {
        _highlightRenderer.enabled = isHighlighted;
    }

    public void PickUp(Transform parent)
    {
        _physical.SetPickedupState(parent);
        _movement.ChangeState(ChickenState.PickedUp);

        _attackZone.enabled = false;

        _state = ChickenState.PickedUp;
    }

    public void Throw(Vector3 force)
    {
        _physical.SetThrownState(force);
        _movement.ChangeState(ChickenState.Thrown);

        _attackZone.enabled = false;

        _state = ChickenState.Thrown;
    }

    public void AddToBattle(ChickenBattle battle)
    {
        if (_battle == battle)
            return;

        if (_battle)
            _battle.RemoveChickenOutOfBattle(this);
        
        _battle = battle;

        if (CanFight())
            ChangeState(ChickenState.Fight);
    }

    public void RemoveFromBattle()
    {
        if (!_battle)
            return;

        _battle.RemoveChickenOutOfBattle(this);
        _battle = null;

        if (_state == ChickenState.Fight)
            ChangeState(ChickenState.Farm);
    }

    public void WakeupFromBattlePause()
    {
        if(!IsPauzedFromBattle())
            return ;

        ChangeState(ChickenState.Fight);
    }

    public bool IsInBattle()
    {
        return _battle != null;
    }

    public bool IsPauzedFromBattle()
    {
        return IsInBattle() && _state != ChickenState.Fight;
    }

    public bool CanFight()
    {
        return _state == ChickenState.Farm || _state == ChickenState.Fight;
    }

    public void PauzeBattle()
    {
        if (_state == ChickenState.Fight)
            ChangeState(ChickenState.Farm);
    }

    public bool IsEnemy(Chicken possibleEnemy)
    {
        if (_battle)
            return _battle.IsEnemy(this, possibleEnemy);
        else
            return false;
    }

    public void Damage(float damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0.0f)
        {
            if (_battle)
            {
                _battle.RemoveChickenOutOfBattle(this);
            }
            Destroy(this.gameObject);
        }
    }
}