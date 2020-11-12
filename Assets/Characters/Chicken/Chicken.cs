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
    public class DiedEventArgs : EventArgs
    {
        private float _destroyDelay = 0.0f;
        public float DestroyDelay { get { return _destroyDelay; } }

        public void RequestDelayBeforeDestroy(float delay)
        {
            if (delay > _destroyDelay)
                _destroyDelay = delay;
        }
    }
    #endregion

    #region --- Events ---
    public event EventHandler<DiedEventArgs> Died;
    #endregion

    //---Components---
    [SerializeField] private ChickenStats _stats = null;
    [SerializeField] private ChickenPhysical _physical = null;
    [SerializeField] private ChickenMovement _movement = null;
    [SerializeField] private CC_Location _location = null;
    [SerializeField] private CC_Fighter _chickenFight = null;
    [SerializeField] private CC_Breeder _breeder = null;
    [SerializeField] private Animator _animator = null;

    //---Variables---

    //---Public---
    public ChickenStats Stats { get { return _stats; } set { _stats = value; } }

    public ChickenPhysical Physical { get { return _physical; } }

    public ChickenMovement Movement { get { return _movement; } }

    public CC_Fighter Fighter { get { return _chickenFight; } }

    public CC_Location Location { get { return _location; } }

    public CC_Breeder Breeder { get { return _breeder; } }

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
        _chickenFight.OutOfHealth += _chickenFight_OutOfHealth;
    }

    private void _chickenFight_OutOfHealth(object sender, CC_Fighter.DamageTakenEventArgs e)
    {
        var args = new DiedEventArgs();
        Died?.Invoke(this, args);
        Destroy(gameObject, args.DestroyDelay);
    }

    private void OnDestroy()
    {
        Died?.Invoke(this, new DiedEventArgs());
    }

    private void Update()
    {
    }
}