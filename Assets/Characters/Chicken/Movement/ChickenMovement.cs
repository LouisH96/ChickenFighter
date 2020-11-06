using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenMovement : MonoBehaviour
{
    //---Components---
    [SerializeField] private MovementAgent _agent = null;

    //---Behaviors---
    [SerializeField] private MovementBehavior _farmBehavior = null;
    [SerializeField] private ChickenFightMovement _chickenFightMovement = null;

    void Awake()
    {
        if (!_agent)
            _agent = GetComponent<MovementAgent>();

        if (!_chickenFightMovement)
            _chickenFightMovement = GetComponent<ChickenFightMovement>();
    }

    void Update()
    {

    }
}
