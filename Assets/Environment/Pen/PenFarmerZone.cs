using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenFarmerZone : MonoBehaviour
{
    //--- Components ---
    [SerializeField] private ChickenPen _pen = null;

    //--- Unity Functions ---
    private void OnTriggerEnter(Collider other)
    {
        Farmer farmer = Farmer.GetFromCollider(other);
        if (!farmer)
            return;

        farmer.PenAreaEntered(_pen);
    }

    private void OnTriggerExit(Collider other)
    {
        Farmer farmer = Farmer.GetFromCollider(other);
        if (!farmer)
            return;

        farmer.PenAreaExit(_pen);
    }
}