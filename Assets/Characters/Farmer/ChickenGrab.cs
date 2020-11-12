using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class ChickenGrab : MonoBehaviour
{
    public event EventHandler<Chicken> ChickenHighlighted;
    public event EventHandler<Chicken> ChickenUnHighlighted;

    public event EventHandler<Chicken> ChickenGrabbed;
    public event EventHandler<Chicken> ChickenThrown;

    [SerializeField] private Farmer _farmer = null;
    [SerializeField] private float _chickenEjectionForce = 100.0f;
    [SerializeField] private Transform[] _hoverLocations = null;
    private Chicken _chickenToPickup = null;

    public Chicken HighlightedChicken { get { return _chickenToPickup; } }

    public int MaxGrabbed { get { return _hoverLocations.Count(); } }
    public int AmntGrabbed { get { return GetPickedUpChickens().Count; } }

    void Update()
    {
        HandlePickupChicken();
        HandleDropChickens();
    }

    private void HandlePickupChicken()
    {
        if (Input.GetAxis("Action1") > 0.0f
            && _chickenToPickup)
        {
            Transform location = GetEmptyGrabLocation();
            Assert.IsNotNull(location); //if there is no location available, _chickenToPickup should be null

            ChickenPhysical chickenPhysical = _chickenToPickup.Physical;
            if (!chickenPhysical)
                return;

            //change chickenMode
            UnsetChickenToPickup();
            chickenPhysical.Grab(location);
            ChickenGrabbed?.Invoke(this, chickenPhysical.Chicken);
        }
    }

    private void HandleDropChickens()
    {
        if (Input.GetAxis("Action2") > 0.0f
            && HasChickensPickedUp())
        {
            foreach (Chicken chicken in GetPickedUpChickens())
            {
                ChickenPhysical chickenPhysical = chicken.Physical;
                if (!chickenPhysical)
                    continue;

                chickenPhysical.Throw(transform.forward * _chickenEjectionForce);
                ChickenThrown?.Invoke(this, chickenPhysical.Chicken);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_chickenToPickup)
            return;

        //is chicken?
        Chicken chicken = ChickenPhysical.GetCharacterChickenFromCollider(other);
        if (!chicken)
            return;

        //not already picked up ?
        Assert.IsFalse(IsChickenAlreadyPickedup(chicken)); //if picked up, should no trigger this again

        //is there an empty location ?
        Transform emptyLocation = GetEmptyGrabLocation();
        if (!emptyLocation)
            return;

        ChickenPhysical chickenPhysical = chicken.Physical;
        if (!chickenPhysical)
            return;

        if (!chickenPhysical.CanGrab())
            return;

        //prepare for pickup
        SetChickenToPickup(chicken);
    }

    private void OnTriggerExit(Collider other)
    {
        //is chicken?
        Chicken chicken = ChickenPhysical.GetCharacterChickenFromCollider(other);
        if (!chicken)
            return;

        if (chicken == _chickenToPickup)
            UnsetChickenToPickup();
    }

    private void SetChickenToPickup(Chicken chicken)
    {
        if (!chicken)
            return;

        if (_chickenToPickup)
            UnsetChickenToPickup();

        _chickenToPickup = chicken;
        _chickenToPickup.Physical.SetHighlighted(true, _farmer);

        ChickenHighlighted?.Invoke(this, chicken);
    }

    private void UnsetChickenToPickup()
    {
        if (!_chickenToPickup)
            return;

        ChickenUnHighlighted?.Invoke(this, _chickenToPickup);

        _chickenToPickup.Physical.SetHighlighted(false, _farmer);
        _chickenToPickup = null;
    }

    public Transform GetEmptyGrabLocation()
    {
        //location is empty if it has no childs
        foreach (Transform location in _hoverLocations)
            if (location.childCount == 0)
                return location;

        return null;
    }

    private bool IsChickenAlreadyPickedup(Chicken chicken)
    {
        foreach (Transform location in _hoverLocations)
            if (location.childCount == 1)
                if (location.GetChild(0) == chicken)
                    return true;

        return false;
    }

    private bool HasChickensPickedUp()
    {
        if (_hoverLocations.Length == 0)
            return false;

        foreach (Transform location in _hoverLocations)
            if (location.childCount > 0)
                return true;

        return false;
    }

    public List<Chicken> GetPickedUpChickens()
    {
        return _hoverLocations
            .Where(l => l.childCount > 0)
            .Select(l => l.GetChild(0))
            .Select(c => c.GetComponent<Chicken>())
            .ToList();
    }
}
