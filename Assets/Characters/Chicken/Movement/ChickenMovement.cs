using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenMovement : MonoBehaviour
{
    //---Components---
    [SerializeField] private MovementAgent _agent = null;

    //---Behaviors---
    [SerializeField] private MovementBehavior _farmBehavior = null;
    [SerializeField] private ChickenFightMovement _chickenFightMovement = null;

    //---Variables---
    private Chicken.ChickenState _chickenState = Chicken.ChickenState.None;

    //---Public---
    public Chicken.ChickenState State { get { return _chickenState; } }
    void Awake()
    {
        if (!_agent)
        {
            _agent = GetComponent<MovementAgent>();
        }

        if (!_chickenFightMovement)
        {
            _chickenFightMovement = GetComponent<ChickenFightMovement>();
        }
    }

    private void Start()
    {
        ChangeState(_chickenState);
    }

    void Update()
    {

    }

    public void  ChangeState(Chicken.ChickenState newState)
    {
        _agent.enabled = newState != Chicken.ChickenState.None;
        _farmBehavior.enabled = newState == Chicken.ChickenState.Farm;
        _chickenFightMovement.enabled = newState == Chicken.ChickenState.Fight;

        if (newState == Chicken.ChickenState.Farm)
            _agent.MovementBehavior = _farmBehavior;

        _chickenState = newState;
    }
}
