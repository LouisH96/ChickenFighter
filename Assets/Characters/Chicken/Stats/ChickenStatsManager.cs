using Assets.Characters.Chicken.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenStatsManager : MonoBehaviour
{
    private static ChickenStatsManager _instance;
    public static ChickenStatsManager Instance { get { return _instance; } }

    [SerializeField] private ChickenStatDefinition _health;
    [SerializeField] private ChickenStatDefinition _healthRegen;
    [SerializeField] private ChickenStatDefinition _damage;
    [SerializeField] private ChickenStatDefinition _speed;
    [SerializeField] private ChickenStatDefinition _acceleration;

    public ChickenStatDefinition Health { get { return _health; } }
    public ChickenStatDefinition HealthRegen { get { return _healthRegen; } }
    public ChickenStatDefinition Damage { get { return _damage; } }
    public ChickenStatDefinition Speed { get { return _speed; } }
    public ChickenStatDefinition Acceleration { get { return _acceleration; } }

    private void Awake()
    {
        _instance = this;
    }
}