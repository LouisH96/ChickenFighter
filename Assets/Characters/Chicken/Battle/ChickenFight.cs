using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using static FightBodyPart;

public class ChickenFight : MonoBehaviour
{
    public class DamageTakenEventArgs : EventArgs
    {
        public Chicken DamageDealer;
        public float Damage;
        public float HealthBefore;
        public float HealthAfter;
        public bool KilledTarget { get { return HealthBefore > 0.0f && HealthAfter <= 0.0f; } }
    }

    public event EventHandler<ChickenBattle2> BattleJoined;
    public event EventHandler<ChickenBattle2> BattleLeft;

    //public event EventHandler<Chicken> EnemyLeft;

    public event EventHandler<DamageTakenEventArgs> DamageTaken;
    public event EventHandler<DamageTakenEventArgs> Died;
    public event EventHandler<Chicken> TargetKilled;

    //--- Components ---
    [SerializeField] private Chicken _chicken;
    [SerializeField] private ChickenPhysical _physical;
    [SerializeField] private ChickenStats _stats;
    [SerializeField] private AttackZone _attackZone;

    //--- Fight Components ---
    private List<FightBodyPart> _bodyParts = new List<FightBodyPart>();
    private Beak2 _beak = null;

    //--- Variables ---
    private ChickenBattle2 _battle = null;
    private bool _isFighting = false;
    private float _currentHealth = 1.0f;
  

    //--- Public Variable Access ---
    public ChickenBattle2 Battle { get { return _battle; } }

    void Start()
    {
        _bodyParts = _physical.gameObject.GetComponentsInChildren<FightBodyPart>().ToList();
        _beak = _physical.gameObject.GetComponentInChildren<Beak2>();
        Assert.IsTrue(_bodyParts.TrueForAll(b => b.gameObject.layer == LayerMask.NameToLayer("FightBodyParts")), "Not all FightBodyPart were of that layer");

        foreach(var bodyPart in _bodyParts)
            bodyPart.Hit += BodyPart_Hit;

        _currentHealth = _stats.Health;

        EnableFightState(false);
    }

    private void BodyPart_Hit(object sender, HitEventArgs e)
    {
        if (_currentHealth <= 0.0f)
            return;

        if (e.Damage <= 0.0f)
            return;

        DamageTakenEventArgs args = new DamageTakenEventArgs
        {
            DamageDealer = e.Attacker,
            Damage = e.Damage,
            HealthBefore = _currentHealth
        };

        _currentHealth -= e.Damage;
        args.HealthAfter = _currentHealth;

        DamageTaken?.Invoke(this, args);

        if (args.KilledTarget)
        {
            Died?.Invoke(this, args);
            e.Attacker.ChickenFight.KilledChicken(_chicken);
        }
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
        _attackZone.RemoveFromTargets(e);
        //if(_battle.IsEnemy(_chicken, e))
            //EnemyLeft?.Invoke(this, e);
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

    public void KilledChicken(Chicken killed)
    {
        TargetKilled?.Invoke(this, killed);
    }
}