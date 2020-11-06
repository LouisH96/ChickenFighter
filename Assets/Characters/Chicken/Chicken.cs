using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class Chicken : MonoBehaviour
{
    public enum ChickenState
    {
        Farm, Fight, None
    }

    //---Components---
    [SerializeField] private ChickenStats _stats = null;
    [SerializeField] private ChickenPhysical _physical = null;
    [SerializeField] private ChickenMovement _movement = null;
    [SerializeField] private FarmChicken2 _farmChicken = null;
    [SerializeField] private FightChicken2 _fightChicken = null;

    //---Variables---
    [SerializeField] private ChickenState _state = ChickenState.None;

    void Awake()
    {
    }

    private void Start()
    {
        ChangeState(_state);
    }

    private void ChangeState(ChickenState newState)
    {
        _movement.ChangeState(newState);

        _farmChicken.enabled = newState == ChickenState.Farm;
        _fightChicken.enabled = newState == ChickenState.Fight;

        _state = newState;
    }
}