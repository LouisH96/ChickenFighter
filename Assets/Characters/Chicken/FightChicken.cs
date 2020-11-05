using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightChicken : MonoBehaviour
{
    //---Components---
    private Animator _animator = null;

    //---Stats---
    [SerializeField] private bool _isAttacking = false;
    [SerializeField] private float _attackRange = 1.0f;

    //---Variables--
    [SerializeField] private FightChicken _enemy = null;


    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {

    }

    void Update()
    {
        if (_enemy && _isAttacking && (transform.position - _enemy.transform.position).sqrMagnitude < _attackRange * _attackRange)
        {
            //attack
            _animator.SetBool("Attack", true);
        }
        else
        {
            _animator.SetBool("Attack", false);
        }
    }
}
