using Assets.Characters.Chicken.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenStatsManager : MonoBehaviour
{
    private static ChickenStatsManager _instance;
    public static ChickenStatsManager Instance { get { return _instance; } }

    [SerializeField] private ChickenStatDefinition _health = new ChickenStatDefinition();
    [SerializeField] private ChickenStatDefinition _healthRegen = new ChickenStatDefinition();
    [SerializeField] private ChickenStatDefinition _damage = new ChickenStatDefinition();
    [SerializeField] private ChickenStatDefinition _speed = new ChickenStatDefinition();
    [SerializeField] private ChickenStatDefinition _acceleration= new ChickenStatDefinition();

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