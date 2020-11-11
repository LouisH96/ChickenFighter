using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class Chicken : MonoBehaviour
{
    #region --- EventArgs ---
    public class DamageTakenEventArgs : EventArgs
    {
        public Chicken DamageDealer;
        public float Damage;
        public float HealthBefore;
        public float HealthAfter;
        public bool KilledTarget { get { return HealthBefore > 0.0f && HealthAfter < 0.0f; } }
    }
    #endregion

    #region --- Events ---
    public event EventHandler<DamageTakenEventArgs> DamageTaken;
    #endregion


    public enum ChickenState
    {
        Farm, Fight, PickedUp, Thrown, None
    }

    //---Components---
    [SerializeField] private ChickenStats _stats = null;
    [SerializeField] private ChickenPhysical _physical = null;
    [SerializeField] private ChickenMovement _movement = null;
    [SerializeField] private CC_Location _location = null;
    [SerializeField] private ChickenFight _chickenFight = null;
    [SerializeField] private Renderer _highlightRenderer = null;
    [SerializeField] private Animator _animator = null;

    //---Variables---
    [SerializeField] private ChickenState _state = ChickenState.None;
    private ChickenBattle _battle = null;
    private float _currentHealth = 50.0f;

    [SerializeField] private float _breedInterval = 100.0f;
    private float _breedTimer = 0.0f;
    private bool _isBreedable = false;

    private static List<Chicken> _allChickens = new List<Chicken>();
    [SerializeField] private float _sqrBreedRange = 4.0f;

    //---Public---
    public ChickenStats Stats { get { return _stats; } set { _stats = value; } }

    public ChickenPhysical Physical { get { return _physical; } }

    public ChickenFight ChickenFight { get { return _chickenFight; } }

    public ChickenBattle Battle { get { return _battle; } }

    public CC_Location Location { get { return _location; } }

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

    public bool isHighLighted
    {
        get { return _highlightRenderer.enabled; }
    }

    public float CurrentHealthRatio { get { return _currentHealth / _stats.Health; } }
    public float CurrentHealth { get { return _currentHealth; } }
    public float MaxHealth { get { return _stats.Health; } }

    void Awake()
    {
        _breedTimer = UnityEngine.Random.Range(0.0f, _breedInterval);
    }

    private void Start()
    {
        ChangeState(_state);
        _currentHealth = _stats.Health;

        _allChickens.Add(this);

        _chickenFight.Died += _chickenFight_Died;
    }

    private void _chickenFight_Died(object sender, ChickenFight.DamageTakenEventArgs e)
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        _currentHealth += _stats.HealthRegen * Time.deltaTime;
        if (_currentHealth > _stats.Health)
            _currentHealth = _stats.Health;

        if (!_isBreedable)
        {
            _breedTimer += Time.deltaTime;
            if (_breedTimer > _breedInterval)
            {
                _isBreedable = true;
            }
        }
        else
        {
            //TryBreed();
        }
    }

    private void TryBreed()
    {
        Chicken partner = _allChickens
            .Where(c => c!= this && c._battle == null)
            .FirstOrDefault(c => (c.transform.position - transform.position).sqrMagnitude < _sqrBreedRange);

        if(partner)
        {
            GameObject childChicken =  Instantiate(this.gameObject);
            _stats.BreedStats(partner.Stats, childChicken.GetComponent<ChickenStats>());

            partner._breedTimer = 0.0f;
            _breedTimer = 0.0f;
            _isBreedable = false;
        }
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
        //_movement.ChangeState(newState);
        //_physical.ChangeState(newState);

        IsAttacking = false;

        _state = newState;
    }

    public void SetHighlight(bool isHighlighted)
    {
        _highlightRenderer.enabled = isHighlighted;
    }

    //public void PickUp(Transform parent)
    //{
    //    _physical.SetPickedupState(parent);
    //    _movement.ChangeState(ChickenState.PickedUp);

    //    _attackZone.enabled = false;

    //    _state = ChickenState.PickedUp;
    //}

    //public void Throw(Vector3 force)
    //{
    //    _physical.SetThrownState(force);
    //    _movement.ChangeState(ChickenState.Thrown);

    //    _attackZone.enabled = false;

    //    _state = ChickenState.Thrown;
    //}

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
        if (!IsPauzedFromBattle())
            return;

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

    private void TakeDamage(float damage)
    {
       // _currentHealth -= damage;

        if (_currentHealth <= 0.0f)
        {
            if (_battle)
            {
                _battle.RemoveChickenOutOfBattle(this);
            }
            _allChickens.Remove(this);
            Destroy(this.gameObject);
        }
    }
}