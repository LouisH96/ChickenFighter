using Assets.Characters.Chicken.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class ChickenStats : MonoBehaviour
{
    public class ChickenStatEventArgs : EventArgs
    {
        public float OldValue;
        public float NewValue;
        public ChickenStatDefinition Stat;
    }

    public event EventHandler<ChickenStatEventArgs> StatUpgraded;

    [SerializeField] private Chicken _chicken = null;
    [SerializeField] private float _health = 50;
    [SerializeField] private float _healthRegen = 1.0f;
    [SerializeField] private float _speed = 3.0f;
    [SerializeField] private float _acceleration = 6.0f;
    [SerializeField] private float _damage = 1.0f;

    //[SerializeField] private int _age = 1;

    public float Health { get { return _health; } }
    public float HealthRegen { get { return _healthRegen; } }
    public float Speed { get { return _speed; } }
    public float Acceleration { get { return _acceleration; } }
    public float Damage { get { return _damage; } }

    // Start is called before the first frame update
    void Start()
    {
        _chicken.Fighter.TargetKilled += ChickenFight_TargetKilled;
    }

    private void ChickenFight_TargetKilled(object sender, Chicken e)
    {
        UpgradeRandomStat();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpgradeRandomStat()
    {
        int statIndex = UnityEngine.Random.Range(0, 5);
        var definition = GetStatDefinition(statIndex);

        UpgradeStat(definition);
    }

    public void UpgradeStat(ChickenStatDefinition definition)
    {
        float oldValue = GetStatValue(definition);
        SetStatValue(definition.UpgradeStat(oldValue), definition);
        float newValue = GetStatValue(definition);

        StatUpgraded?.Invoke(this, new ChickenStatEventArgs
        {
            Stat = definition,
            OldValue = oldValue,
            NewValue = newValue
        });
    }

    public float GetStatValue(ChickenStatDefinition definition)
    {
        ChickenStatsManager manager = ChickenStatsManager.Instance;
        if (definition == manager.Health)
            return _health;
        else
        if (definition == manager.HealthRegen)
            return _healthRegen;
        else
        if (definition == manager.Damage)
            return _damage;
        else
        if (definition == manager.Speed)
            return _speed;
        else
            if (definition == manager.Acceleration)
            return _acceleration;
        else
            return -1.0f;
    }

    public void SetStatValue(float newValue, ChickenStatDefinition definition)
    {
        ChickenStatsManager manager = ChickenStatsManager.Instance;
        if (definition == manager.Health)
            _health = newValue;
        else
        if (definition == manager.HealthRegen)
            _healthRegen = newValue;
        else
        if (definition == manager.Damage)
            _damage = newValue;
        else
        if (definition == manager.Speed)
            _speed = newValue;
        else
            if (definition == manager.Acceleration)
            _acceleration = newValue;
    }

    private ChickenStatDefinition GetStatDefinition(int statIndex)
    {
        ChickenStatsManager manager = ChickenStatsManager.Instance;
        switch (statIndex)
        {
            case 0: return manager.Health;
            case 1: return manager.HealthRegen;
            case 2: return manager.Speed;
            case 3: return manager.Acceleration;
            case 4: return manager.Damage;
        }
        return null;
    }

    public void BreedStats(ChickenStats partner, ChickenStats child)
    {
        ChickenStatsManager manager = ChickenStatsManager.Instance;

        child._health = manager.Health.BreedStat(this.Health, partner.Health);
        child._healthRegen = manager.HealthRegen.BreedStat(this.HealthRegen, partner.HealthRegen);
        child._damage = manager.Damage.BreedStat(this.Damage, partner.Damage);
        child._speed = manager.Speed.BreedStat(this.Speed, partner.Speed);
        child._acceleration = manager.Acceleration.BreedStat(this.Acceleration, partner.Acceleration);
    }
}