using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SeekBehavior))]
[RequireComponent(typeof(MovementAgent))]
public class FightChicken : MonoBehaviour
{
    //---Components---
    private ArriveBehavior _arriveBehavior = null;
    private Animator _animator = null;
    private MovementAgent _movementAgent = null;

    //---Variables---
   [SerializeField] private FightChicken _target = null;
    public FightChicken Target
    {
        get { return _target; }
        set { _target = value; if (_arriveBehavior) _arriveBehavior.LockedTarget = _target.transform; }
    }

    void Awake()
    {
        _arriveBehavior = GetComponent<ArriveBehavior>();
        _animator = GetComponent<Animator>();
        _movementAgent = GetComponent<MovementAgent>();
    }

    private void Start()
    {
        if (_arriveBehavior && _target)
            _arriveBehavior.LockedTarget = _target.transform;
    }

    void Update()
    {
        if (_arriveBehavior && _target && _arriveBehavior.IsTargetReached(_movementAgent))
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
