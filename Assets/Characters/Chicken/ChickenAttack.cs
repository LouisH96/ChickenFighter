using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenAttack : MonoBehaviour
{
    //---Components---
    private Animator _animator;
    private AttackZone _attackZone = null;

    [SerializeField] private bool _tryAttacking = true;
    

    void Awake()
    {
        _animator = GetComponent<Animator>();
        if (!_attackZone) _attackZone = GetComponent<AttackZone>();
        if (!_attackZone) _attackZone = GetComponentInChildren<AttackZone>();
    }

    void Update()
    {
        bool canAttack = _tryAttacking && _animator && _attackZone && _attackZone.Enemy;
        _animator.SetBool("Attack", canAttack);
    }
}