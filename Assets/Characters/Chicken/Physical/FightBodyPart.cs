using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Rigidbody))]
public class FightBodyPart : MonoBehaviour
{
    //---Events---
    public class HitEventArgs : EventArgs
    {
        public Chicken Attacked;
        public Chicken Attacker;
        public FightBodyPart BodyPart;
        public float Damage;
    }

    public event EventHandler<HitEventArgs> Hit;

    //---Components---
    [SerializeField] private Rigidbody _rigidbody = null;
    [SerializeField] private Collider _collider = null;
    [SerializeField] private Chicken _chicken = null;

    //---Stats---

    //---Public---
    public Chicken Chicken { get { return _chicken; } }

    void Awake()
    {
        if (!_rigidbody) _rigidbody = GetComponent<Rigidbody>();
        if (!_collider) _collider = GetComponent<Collider>();

        Assert.IsNotNull(_collider, this.name + " has no collider");

        if (!_chicken)
            _chicken = GetComponentInParent<Chicken>();

        Chicken.ChickenFight.DamageTaken += ChickenFight_DamageTaken;
    }

    private void ChickenFight_DamageTaken(object sender, ChickenFight.DamageTakenEventArgs e)
    {

    }

    public void HitBodyPart(float damage, Chicken damageDealer)
    {
        Hit?.Invoke(this, new HitEventArgs
        {
            Attacked = _chicken,
            Attacker = damageDealer,
            BodyPart = this,
            Damage = damage
        });
    }

    private void OnEnable()
    {
        _collider.enabled = true;
    }

    private void OnDisable()
    {
        _collider.enabled = false;
    }
}