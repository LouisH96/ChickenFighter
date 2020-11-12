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
    #endregion

    #region --- Events ---
    #endregion

    //---Components---
    [SerializeField] private ChickenStats _stats = null;
    [SerializeField] private ChickenPhysical _physical = null;
    [SerializeField] private ChickenMovement _movement = null;
    [SerializeField] private CC_Location _location = null;
    [SerializeField] private CC_Fighter _chickenFight = null;
    [SerializeField] private Animator _animator = null;

    //---Variables---

    //---Public---
    public ChickenStats Stats { get { return _stats; } set { _stats = value; } }

    public ChickenPhysical Physical { get { return _physical; } }

    public ChickenMovement Movement { get { return _movement; } }

    public CC_Fighter Fighter { get { return _chickenFight; } }

    public CC_Location Location { get { return _location; } }

    public bool IsAttacking
    {
        get { return _animator.GetBool("Attack"); }
        set { _animator.SetBool("Attack", value); }
    }

    void Awake()
    {
    }

    private void Start()
    {
        _chickenFight.Died += _chickenFight_Died;
    }

    private void _chickenFight_Died(object sender, CC_Fighter.DamageTakenEventArgs e)
    {
        Destroy(gameObject);
    }

    private void Update()
    {
    }
}