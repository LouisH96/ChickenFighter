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

    //---Variables---
    [SerializeField] private ChickenState _state = ChickenState.None;
    private ChickenBattle _battle = null;

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

    void Awake()
    {
    }

    private void Start()
    {
        ChangeState(_state);
    }

    public void ChangeState(ChickenState newState)
    {
        _movement.ChangeState(newState);
        _physical.ChangeState(newState);

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

        _state = ChickenState.PickedUp;
    }

    public void Throw(Vector3 force)
    {
        _physical.SetThrownState(force);
        _movement.ChangeState(ChickenState.Thrown);

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
}