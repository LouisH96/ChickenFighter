using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Rigidbody))]
public class FightBodyPart : MonoBehaviour
{
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