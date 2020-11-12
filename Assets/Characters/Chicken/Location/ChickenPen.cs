using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;

public class ChickenPen : MonoBehaviour
{
    //--- Events ---
    public event EventHandler<Chicken> ChickenAdded;
    public event EventHandler<Chicken> ChickenRemoved;

    //--- Stats ---
    [SerializeField] private int _maxChickens = 10;

    //--- Variables ---
    private List<Chicken> _chickens = new List<Chicken>();
    
    //--- Public Member Access ---
    public ReadOnlyCollection<Chicken> Chickens { get { return _chickens.AsReadOnly(); } }

    public void AddChicken(Chicken chicken)
    {
        if (!_chickens.Contains(chicken))
        {
            _chickens.Add(chicken);
            ChickenAdded?.Invoke(this, chicken);
        }
        else
        {
            Assert.IsTrue(true, "chicken is already added to pen, so cannot add it");
        }
    }
    public void RemoveChicken(Chicken chicken)
    {
        if (_chickens.Contains(chicken))
        {
            _chickens.Remove(chicken);
            ChickenRemoved?.Invoke(this, chicken);
        }
        else
        {
            Assert.IsTrue(true, "chicken is not added to pen, so cannot delete it");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Chicken chicken = ChickenPhysical.GetChicken(other);

        if (!chicken)
            return;

        if (_chickens.Contains(chicken))
            return;

        Assert.IsFalse(_chickens.Contains(chicken), "chicken should not be in pen");

        CC_Location chickenLocation = chicken.Location;

        if (!chickenLocation)
            return;

        chickenLocation.EnterPen(this);

    }
    private void OnTriggerExit(Collider other)
    {
        Chicken chicken = ChickenPhysical.GetChicken(other);
        if (!chicken)
            return;

        CC_Location chickenLocation = chicken.Location;
        if (!chickenLocation)
            return;

        chickenLocation.ExitPen(this);
    }
}