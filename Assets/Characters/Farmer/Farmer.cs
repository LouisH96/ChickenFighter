using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class Farmer : MonoBehaviour
{
    //---Components---
    [SerializeField] private ChickenGrab _chickenGrab = null;
    [SerializeField] private UIManager _uiManager = null;

    //--- Variables ---
    private List<ChickenPen> _closePens = new List<ChickenPen>();
    private ChickenPen _displayedPen = null;

    //--- Public Member Access ---
    public ChickenGrab ChickenGrab { get { return _chickenGrab; } }
    public UIManager UIManager { get { return _uiManager; } }

    //--- Unity Functions ---
    private void Update()
    {
        ChickenPen closest = null;

        if (_closePens.Count > 0)
        {
            float sqrClosestDist = 0.0f;

            foreach(var pen in _closePens)
            {
                float sqrPenDist = (pen.transform.position - transform.position).sqrMagnitude;

                if(closest == null
                    || sqrPenDist < sqrClosestDist)
                {
                    closest = pen;
                    sqrClosestDist = sqrPenDist;
                }
            }
        }

        if (closest)
            DisplayPen(closest);
        else
            UndisplayPen();
    }

    //--- Public Functions ---
    public void PenAreaEntered(ChickenPen pen)
    {
        if (_closePens.Contains(pen))
            return;

        _closePens.Add(pen);
    }

    public void PenAreaExit(ChickenPen pen)
    {
        if (!_closePens.Contains(pen))
            return;
        _closePens.Remove(pen);

        if (_displayedPen == pen)
            UndisplayPen();
    }

    //--- Public Static Functions ---
    public static Farmer GetFromCollider(Collider collider)
    {
        Farmer farmer = null;

        if (collider.CompareTag("Player"))
            farmer = collider.gameObject.GetComponent<Farmer>();

        return farmer;
    }

    //--- Private Functions ---
    private void DisplayPen(ChickenPen pen)
    {
        if (!pen || _displayedPen == pen)
            return;

        UndisplayPen();
        _uiManager.DisplayPen(pen);

        _displayedPen = pen;
    }

    private void UndisplayPen()
    {
        if (!_displayedPen)
            return;

        _uiManager.UndisplayPen();
        _displayedPen = null;
    }
}
