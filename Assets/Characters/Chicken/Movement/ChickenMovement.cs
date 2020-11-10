using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenMovement : MonoBehaviour
{
    //--- State Enum ---
    //---Components---
    [SerializeField] private Chicken _chicken = null;
    [SerializeField] private MovementAgent _agent = null;

    //---Behaviors---
    [SerializeField] private MovementBehavior _farmBehavior = null;
    [SerializeField] private ChickenFightMovement _chickenFightMovement = null;

    //---Variables---
    private Chicken.ChickenState _chickenState = Chicken.ChickenState.None;

    //---Public---
    public Chicken.ChickenState State { get { return _chickenState; } }
    public MovementAgent Agent { get { return _agent; } }

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
        _farmBehavior.enabled = newState == Chicken.ChickenState.Farm;
        _chickenFightMovement.enabled = newState == Chicken.ChickenState.Fight;
        _agent.enabled = newState != Chicken.ChickenState.None;

        if (newState == Chicken.ChickenState.Farm)
        {
            _agent.MovementBehavior = _farmBehavior;
            _agent.MaxVelocity = _chicken.Stats.MaxSpeed * 0.75f;
            _agent.Acceleration = _chicken.Stats.Acceleration * 0.75f;
        }
        else if (newState==Chicken.ChickenState.Fight)
        {
            _agent.MaxVelocity = _chicken.Stats.MaxSpeed;
            _agent.Acceleration = _chicken.Stats.Acceleration;
        }
        else if (newState == Chicken.ChickenState.PickedUp)
            _agent.MovementBehavior = null;

        _chickenState = newState;
    }
}
