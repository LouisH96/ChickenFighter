using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class Chicken : MonoBehaviour
{
    //---Components---
    private ChickenStats _stats = null;
    private ChickenPhysical _physical = null;
    private ChickenMovement _movement = null;

    //---Public---
    

    void Awake()
    {
        _stats = GetComponent<ChickenStats>();
        _physical = GetComponent<ChickenPhysical>();
        _movement = GetComponent<ChickenMovement>();

    }
}
