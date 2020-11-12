using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmer : MonoBehaviour
{
    //---Components---
    [SerializeField] private ChickenGrab _chickenGrab = null;

    //--- Public Member Access ---
    public ChickenGrab ChickenGrab { get { return _chickenGrab; } }
}
