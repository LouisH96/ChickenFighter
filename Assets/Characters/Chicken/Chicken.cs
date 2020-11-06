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
    public ChickenStats Stats { get{ return _stats; } }

    public ChickenBattle Battle { get { return _battle; } }

    public Chicken BattleEnemy
    {
        get
        {
            if (_battle)
                return _battle.GetEnemy(this);
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

    public void StartBattle(ChickenBattle battle)
    {
        if (_battle)
            _battle.EndBattle();

        _battle = battle;

        ChangeState(ChickenState.Fight);
    }

    public void EndBattle()
    {
        if (_battle)
            _battle.EndBattle();

        _battle = null;

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

        Debug.Log("Damage "+_currentHealth);
        if (_currentHealth <= 0.0f)
        {
            if (_battle)
                _battle.EndBattle();
            Destroy(this.gameObject);
        }
    }
}