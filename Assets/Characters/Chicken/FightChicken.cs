using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SeekBehavior))]
public class FightChicken : MonoBehaviour
{
    //---Components---
    private SeekBehavior _seekBehavior = null;
    private Animator _animator = null;

    //---Variables---
   [SerializeField] private FightChicken _target = null;
    public FightChicken Target
    {
        get { return _target; }
        set { _target = value; if (_seekBehavior) _seekBehavior.LockTarget = _target.transform; }
    }

    void Awake()
    {
        _seekBehavior = GetComponent<SeekBehavior>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (_seekBehavior && _target)
            _seekBehavior.LockTarget = _target.transform;
    }

    void Update()
    {
        if (_seekBehavior && _target && _seekBehavior.TargetReached)
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
