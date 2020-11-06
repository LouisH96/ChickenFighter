using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightChicken2 : MonoBehaviour
{
    //---Components---
    [SerializeField] private ChickenStats _stats = null;
    [SerializeField] private ChickenPhysical _physical = null;
    [SerializeField] private ChickenMovement _movement = null;

    private void OnEnable()
    {

    }

}
