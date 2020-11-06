using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmChicken2 : MonoBehaviour
{
    //---Components---
    [SerializeField] private ChickenStats _stats = null;
    [SerializeField] private ChickenPhysical _physical = null;
    [SerializeField] private ChickenMovement _movement = null;

    //---Public---

    void Awake()
    {

    }

    private void OnEnable()
    {
    }
}
