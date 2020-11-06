using System;
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

    [SerializeField] private Chicken _chicken = null;

    //[SerializeField] private int _age = 1;

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

    public void UpgradeRandomStat()
    {
        int stat = UnityEngine.Random.Range(0, 5);

        if(stat== 0)
        {
            _health += 10;
            _chicken.OnMaxHealthUpgraded(10);
        }
        else if(stat == 1)
        {
            _healthRegen += 0.5f;
        }
        else if(stat == 2)
        {
            _maxSpeed += 0.5f;
            _chicken.OnMaxSpeedUpgraded();
        }
        else if(stat == 3)
        {
            _acceleration += 0.5f;
            _chicken.OnAccelerationUpgraded();
        }
        else if(stat == 4)
        {
            _damage += 1.0f;
        }
    }
}
