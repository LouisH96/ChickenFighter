using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FightChicken : MonoBehaviour
{
    //---Components---
    //private Animator _animator = null;

    //---Stats---
    //[SerializeField] private bool _isAttacking = false;
    //[SerializeField] private float _attackRange = 1.0f;

    //---Variables--
    //[SerializeField] private FightChicken _enemy = null;


    void Awake()
    {
        AddTagRecursively(transform, tag);
        //  _animator = GetComponent<Animator>();
    }

    private void Start()
    {
    }

    void Update()
    {
        //if (_enemy && _isAttacking && (transform.position - _enemy.transform.position).sqrMagnitude < _attackRange * _attackRange)
        //{
        //    //attack
        //    _animator.SetBool("Attack", true);
        //}
        //else
        //{
        //    _animator.SetBool("Attack", false);
        //}
    }

    void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        //if (_isAttacking)
        //{
        //    Handles.color = Color.cyan;
        //    Handles.DrawWireDisc(transform.position, Vector3.up, _attackRange);
        //}
#endif
    }

    //from the internets
    void AddTagRecursively(Transform trans, string tag)
    {
        trans.gameObject.tag = tag;
        if (trans.childCount > 0)
            foreach (Transform t in trans)
                AddTagRecursively(t, tag);
    }
}