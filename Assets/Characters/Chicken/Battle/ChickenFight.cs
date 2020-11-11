using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class ChickenFight : MonoBehaviour
{
    public event EventHandler<ChickenBattle2> BattleJoined;
    public event EventHandler<ChickenBattle2> BattleLeft;

    public event EventHandler<Chicken> EnemyLeft;

    //--- Components ---
    [SerializeField] private Chicken _chicken;
    [SerializeField] private ChickenPhysical _physical;

    //--- Fight Components ---
    private List<FightBodyPart> _bodyParts = new List<FightBodyPart>();
    private Beak2 _beak = null;

    //--- Variables ---
    private ChickenBattle2 _battle = null;
    private bool _isFighting = false;

    //--- Public Variable Access ---
    public ChickenBattle2 Battle { get { return _battle; } }

    void Start()
    {
        _bodyParts = _physical.gameObject.GetComponentsInChildren<FightBodyPart>().ToList();
        _beak = _physical.gameObject.GetComponentInChildren<Beak2>();

        Assert.IsTrue(_bodyParts.TrueForAll(b => b.gameObject.layer == LayerMask.NameToLayer("FightBodyParts")), "Not all FightBodyPart were of that layer");
    }

    public void JoinBattle(ChickenBattle2 joiningBattle)
    {
        Assert.IsNull(_battle, "cannot join 2 battles");
        _battle = joiningBattle;

        EnableFightState(true);
        BattleJoined?.Invoke(this, _battle);

        _battle.ChickenLeft += _battle_ChickenLeft;
    }

    private void _battle_ChickenLeft(object sender, Chicken e)
    {
        if(_battle.IsEnemy(_chicken, e))
            EnemyLeft?.Invoke(this, e);
    }

    public void LeaveBattle(ChickenBattle2 leavingBattle)
    {
        Assert.AreEqual(_battle, leavingBattle, "leaving a battle chicken is not in");
        _battle.ChickenLeft -= _battle_ChickenLeft;
        _battle = null;

        EnableFightState(false);
        BattleLeft?.Invoke(this, leavingBattle);
    }

    private void EnableFightState(bool enable)
    {
        foreach (var bodyPart in _bodyParts)
            bodyPart.enabled = enable;
        _beak.enabled = enable;

        _isFighting = enable;
    }
}