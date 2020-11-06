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

    //---Stats---
    private float _awakeRigidbodyMass = -1.0f;

    void Awake()
    {
        if (!_rigidbody) _rigidbody = GetComponent<Rigidbody>();
        if (!_collider) _collider = GetComponent<Collider>();

        Assert.IsNotNull(_collider, this.name + " has no collider");
    }

    private void OnDisable()
    {
        
    }
}