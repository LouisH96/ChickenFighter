using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

//class made to enable or disable certain gizmos
//so it isn't clogged when there are alot of gizmos
public class GizmoManager : MonoBehaviour
{
    //Instance (singleton)
    private static GizmoManager _instance;
    public static GizmoManager Instance
    {
        get { return _instance; }
    }

    //Settings
    public bool DrawWanderCircle = false;

    //Set instance
    private void Awake()
    {
        _instance = this;
    }
}