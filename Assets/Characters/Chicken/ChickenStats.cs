using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenStats : MonoBehaviour
{
    [SerializeField] private float _health = 50;
    [SerializeField] private float _healthRegen = 1.0f;
    [SerializeField] private float _maxSpeed = 3.0f;
    [SerializeField] private float _acceleration = 6.0f;
    [SerializeField] private float _damage = 1.0f;

    [SerializeField] private int _age = 1;

    public float Health { get { return _health; } }
    public float HealthRegen { get { return _healthRegen; } }
    public float MaxSpeed { get { return _maxSpeed; } }
    public float Acceleration { get { return _acceleration; } }
    public float Damage { get { return _damage; } }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
