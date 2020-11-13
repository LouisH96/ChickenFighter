using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmer : MonoBehaviour
{
    //---Components---
    [SerializeField] private ChickenGrab _chickenGrab = null;
    [SerializeField] private UIManager _uiManager = null;

    //--- Public Member Access ---
    public ChickenGrab ChickenGrab { get { return _chickenGrab; } }
    public UIManager UIManager { get { return _uiManager; } }


    public static Farmer GetFromCollider(Collider collider)
    {
        Farmer farmer = null;

        if (collider.CompareTag("Player"))
            farmer = collider.gameObject.GetComponent<Farmer>();

        return farmer;
    }
}
