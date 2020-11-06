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
    [SerializeField] private FarmChicken2 _farmChicken = null;
    [SerializeField] private FightChicken2 _fightChicken = null;
    [SerializeField] private Renderer _highlightRenderer = null;

    //---Variables---
    [SerializeField] private ChickenState _state = ChickenState.None;

    //---Public---
    public ChickenStats Stats { get{ return _stats; } }

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

        _farmChicken.enabled = newState == ChickenState.Farm;
        _fightChicken.enabled = newState == ChickenState.Fight;

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
}