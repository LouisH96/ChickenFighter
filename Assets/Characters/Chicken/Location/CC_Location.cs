using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Assertions;
using static Chicken;

//ChickenComponent_Location
public class CC_Location : MonoBehaviour
{
    //--- Components ---
    [SerializeField] private Chicken _chicken = null;

    //--- Variables ---
    private ChickenPen _currentPen = null;
    private bool _isAddedToPen = false;
    private bool _isGrabbed = false;

    void Awake()
    {
        _chicken.Physical.Grabbed += Physical_Grabbed;
        _chicken.Physical.Thrown += Physical_Thrown;
        _chicken.Physical.Landed += Physical_Landed;
    }

    private void Start()
    {
        _chicken.Died += _chicken_Died;
    }

    private void _chicken_Died(object sender, DiedEventArgs e)
    {
        Chicken chicken = e.Chicken;
        chicken.Died -= _chicken_Died;

        if (_currentPen)
            ExitPen(_currentPen);
    }

    private void Physical_Landed(object sender, Chicken e)
    {
        Assert.IsFalse(_isAddedToPen, "cannot be already added to pen & land");
        Assert.IsFalse(_isGrabbed, "cannot land and be grabbed at the same time");
        if (_currentPen)
        {
            _currentPen.AddChicken(_chicken);
            _isAddedToPen = true;
        }
    }

    public void EnterPen(ChickenPen newPen)
    {
        Assert.IsFalse(_isGrabbed, "cannot set pen if grabbed by farmer");
        Assert.IsFalse(_isAddedToPen, "cannot enter pen if already added to other");

        if (_currentPen != newPen)
        {
            Assert.IsNull(_currentPen, "cannot set pen if one is already set");
            _currentPen = newPen;

            if (_chicken.Physical.State == ChickenPhysical.PhysicalState.Character)
            {
                _currentPen.AddChicken(_chicken);
                _isAddedToPen = true;
            }
        }
    }

    public void ExitPen(ChickenPen oldPen)
    {
        Assert.AreEqual(_currentPen, oldPen, "cannot exit a pen if not in it");

        if (_isAddedToPen)
        {
            _currentPen.RemoveChicken(_chicken);
            _isAddedToPen = false;
        }

        _currentPen = null;
    }
    private void Physical_Grabbed(object sender, ChickenPhysical.GrabbedEventArgs e)
    {
        if (_currentPen)
            ExitPen(_currentPen);

        _isGrabbed = true;
    }

    private void Physical_Thrown(object sender, ChickenPhysical.ThrownEventArgs e)
    {
        Assert.IsTrue(_isGrabbed, "cannot throw if not grabbed first");
        Assert.IsFalse(_isAddedToPen, "cannot be thrown if already added to pen (means it is landed in the pen)");
        _isGrabbed = false;
    }
}