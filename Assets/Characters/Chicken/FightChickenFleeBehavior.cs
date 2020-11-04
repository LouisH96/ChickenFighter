using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(FleeBehavior))]
[RequireComponent(typeof(WallAvoidance))]
public class FightChickenFleeBehavior : MonoBehaviour
{
    //---Components---
    private SeekBehavior _seek;
    private FleeBehavior _flee;
    private WallAvoidance _wallAvoidance;

    private bool _isAvoidingWall = false;

    void Awake()
    {
        _seek = GetComponent<SeekBehavior>();
        _flee = GetComponent<FleeBehavior>();
        _wallAvoidance = GetComponent<WallAvoidance>();
    }

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Assert.IsNotNull(_flee);
        Assert.IsNotNull(_wallAvoidance);

        if (_isAvoidingWall)
        {
            if (!_wallAvoidance.HasWallHit && _seek.TargetReached)
            {
                _isAvoidingWall = false;
                _flee.enabled = true;
                _seek.enabled = false;
                _isAvoidingWall = false;
            }
            else
            {
                _seek.Target = _wallAvoidance.EscapePoint;
            }
        }
        else
        {
            if (_wallAvoidance.HasWallHit)
            {
                _flee.enabled = false;
                _seek.Target = _wallAvoidance.EscapePoint;
                _seek.enabled = true;
                _isAvoidingWall = true;
            }
        }


        //if (_isAvoidingWall)
        //{
        //    if (_seek.TargetReached)
        //    {
        //        _flee.enabled = true;
        //        _seek.enabled = false;
        //        _isAvoidingWall = false;
        //    }
        //}
        //else if (_wallAvoidance.HasWallHit)
        //{
        //    _flee.enabled = false;
        //    _seek.Target = _wallAvoidance.EscapePoint;
        //    _seek.enabled = true;
        //    _isAvoidingWall = true;
        //}
        //else
        //{
        //    _flee.enabled = true;
        //    _seek.enabled = false;
        //}
    }

    private void OnEnable()
    {
        _flee.enabled = true;
        _wallAvoidance.enabled = true;
        _isAvoidingWall = false;
        _seek.enabled = false;
    }

    private void OnDisable()
    {
        _flee.enabled = false;
        _wallAvoidance.enabled = false;
        _isAvoidingWall = false;
    }
}
