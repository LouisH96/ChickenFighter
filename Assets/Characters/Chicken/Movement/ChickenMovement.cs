using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenMovement : MonoBehaviour
{
    //--- State Enum ---
    public enum State
    {
        Begin, Wander, Fight, Idle
    }

    //---Components---
    [SerializeField] private Chicken _chicken = null;
    [SerializeField] private MovementAgent _agent = null;

    //---Behaviors---
    [SerializeField] private MovementBehavior _farmBehavior = null;
    [SerializeField] private ChickenFightMovement _chickenFightMovement = null;

    //---Variables---
    private ChickenMovement.State _currentState = ChickenMovement.State.Begin;

    //---Public---
    public ChickenMovement.State CurrentState { get { return _currentState; } }

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

        _chicken.Physical.StateChanged += Physical_StateChanged;
    }

    private void Start()
    {
        _chicken.Fighter.BattleJoined += ChickenFight_BattleJoined;
        _chicken.Fighter.BattleLeft += ChickenFight_BattleLeft;

        if (_currentState == State.Begin)
            ChangeState(State.Wander);
    }

    private void ChickenFight_BattleLeft(object sender, Battle e)
    {
        if (_currentState == State.Fight)
            ChangeState(State.Wander);
    }

    private void ChickenFight_BattleJoined(object sender, Battle e)
    {
        ChangeState(State.Fight);
    }

    private void Physical_StateChanged(object sender, ChickenPhysical.StateChangedEventArgs e)
    {
        if (e.NewState == ChickenPhysical.PhysicalState.Character)
        {
            if (_currentState == State.Idle)
                ChangeState(State.Wander);
        }
        else
            ChangeState(State.Idle);
    }

    #region --- MovementState ---

    public void ChangeState(ChickenMovement.State newState)
    {
        if (_currentState == newState)
            return;

        _currentState = newState;

        EnableWanderState(newState == State.Wander);
        EnableFightState(newState == State.Fight);
        EnableIdleState(newState == State.Idle);
    }

    private void EnableWanderState(bool enable)
    {
        if (enable)
            _agent.MovementBehavior = _farmBehavior;

        _farmBehavior.enabled = enable;
    }

    private void EnableFightState(bool enable)
    {
        _chickenFightMovement.enabled = enable;
    }

    private void EnableIdleState(bool enable)
    {
        _agent.enabled = !enable;

        if (enable)
            _agent.MovementBehavior = null;
    }
    #endregion
}
