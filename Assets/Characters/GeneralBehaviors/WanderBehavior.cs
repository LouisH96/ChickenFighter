using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(SeekBehavior))]
public class WanderBehavior : MonoBehaviour
{
    //---Components---
    private SeekBehavior _seekBehavior = null;

    //---Stats---
    [SerializeField] private float _offset = 1.0f;
    [SerializeField] private float _radius = 1.0f;
    [SerializeField] private float _maxChange = MyUtils.ToRadians(10.0f); //in radians

    //---Variables---
    private float _currentAngle = 0.0f; //in radians

    private void Awake()
    {
        _seekBehavior = GetComponent<SeekBehavior>();
    }

    void Update()
    {
        ChangeAngle();

        Vector3 target = transform.position;
        target += transform.forward * _offset;
        target += MyUtils.DegreesToVectorXZ(_currentAngle) * _radius;

        _seekBehavior.Target = target;
    }

    void ChangeAngle()
    {
        _currentAngle += Random.Range(-_maxChange, _maxChange);
        _currentAngle = MyUtils.ClampRadians(_currentAngle);
    }


#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Vector3 target = transform.position;
        target += transform.forward * _offset;

        Handles.color = Color.white;
        Handles.DrawWireDisc(target, Vector3.up, _radius);

        target += MyUtils.DegreesToVectorXZ(_currentAngle) * _radius;
        Handles.DrawSolidDisc(target, Vector3.up, 0.05f);
    }
#endif
}