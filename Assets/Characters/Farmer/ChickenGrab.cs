using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using static Chicken;

public class ChickenGrab : MonoBehaviour
{
    //--- Events ---
    public event EventHandler<Chicken> ChickenHighlighted;
    public event EventHandler<Chicken> ChickenUnHighlighted;

    public event EventHandler<Chicken> ChickenGrabbed;
    public event EventHandler<Chicken> ChickenThrown;

    //--- Components ---
    [SerializeField] private Farmer _farmer = null;
    [SerializeField] private Transform[] _hoverLocations = null;

    //--- Stats ---
    [SerializeField] private float _chickenEjectionForce = 100.0f;

    //--- Variables ---
    private Chicken _highlightedChicken = null;
    private List<Chicken> _pickupQueue = new List<Chicken>();

    //--- Public Member Access ---
    public Chicken HighlightedChicken { get { return _highlightedChicken; } }
    public int MaxGrabbed { get { return _hoverLocations.Count(); } }
    public int AmntGrabbed { get { return GetPickedUpChickens().Count(); } }

    void Update()
    {
        HandlePickupChicken();
        HandleDropChickens();
    }

    private void HandlePickupChicken()
    {
        if (Input.GetAxis("Action1") > 0.0f
            && HighlightedChicken)
        {
            Transform location = GetEmptyGrabLocation();
            Assert.IsNotNull(location); //if there is no location available, _chickenToPickup should be null

            ChickenPhysical chickenPhysical = HighlightedChicken.Physical;

            //change chickenMode
            UnHighlightChicken();
            chickenPhysical.Grab(location);
            ChickenGrabbed?.Invoke(this, chickenPhysical.Chicken);
            TryHightlightNext();
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

                chicken.Died -= Chicken_Died;

                chickenPhysical.Throw(transform.forward * _chickenEjectionForce);
                ChickenThrown?.Invoke(this, chickenPhysical.Chicken);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //is chicken?
        Chicken chicken = ChickenPhysical.GetCharacterChickenFromCollider(other);
        if (!chicken)
            return;

        //not already in grab-system ?
        Assert.IsFalse(_highlightedChicken == chicken, "chicken already highlighted");
        Assert.IsFalse(_pickupQueue.Contains(chicken), "chicken already in queue");
        Assert.IsFalse(IsChickenAlreadyPickedup(chicken), "chicken already grabbed"); //if picked up, should no trigger this again

        if (!CouldBePickedUp(chicken))
            return;

        _pickupQueue.Add(chicken);
        chicken.Died += Chicken_Died;

        if (!_highlightedChicken)
            TryHightlightNext();
    }

    private void OnTriggerExit(Collider other)
    {
        //is chicken?
        Chicken chicken = ChickenPhysical.GetCharacterChickenFromCollider(other);
        if (!chicken)
            return;

        chicken.Died -= Chicken_Died;

        if (_highlightedChicken == chicken)
        {
            Assert.IsFalse(_pickupQueue.Contains(_highlightedChicken), "highlighted chicken should not be in queue");
            UnHighlightChicken();
            TryHightlightNext();
        }
        else
        {
            Assert.IsTrue(_pickupQueue.Contains(chicken), "chicken cannot not exit trigger, if not in system first");
            _pickupQueue.Remove(chicken);
        }
    }

    private void Chicken_Died(object sender, DiedEventArgs e)
    {
        Chicken chicken = e.Chicken;
        chicken.Died -= Chicken_Died;

        if (_pickupQueue.Contains(chicken))
        {
            _pickupQueue.Remove(chicken);
            Assert.AreNotEqual(_highlightedChicken, chicken, "chicken cant be the highlighted one and in queue");
        }
        else if (_highlightedChicken == chicken)
        {
            UnHighlightChicken();
            TryHightlightNext();
        }
        else if(IsChickenAlreadyPickedup(chicken))
            Debug.LogWarning("Chickengrab should not be subscribed to death event of this chicken");
    }

    private void TryHightlightNext()
    {
        if (_pickupQueue.Count == 0)
            return;

        if (GetEmptyGrabLocation() == null)
            return;

        if (_highlightedChicken)
            UnHighlightChicken();

        foreach (var chicken in _pickupQueue)
        {
            if (!chicken.Physical.CanGrab())
                continue;

            //make chicken the highlighted one
            _pickupQueue.Remove(chicken);
            chicken.Physical.SetHighlighted(true, _farmer);
            _highlightedChicken = chicken;
            ChickenHighlighted?.Invoke(this, _highlightedChicken);
            return;
        }
    }

    private void UnHighlightChicken()
    {
        Assert.IsNotNull(_highlightedChicken, "cannot unhighlight chicken if there is none");

        _highlightedChicken.Physical.SetHighlighted(false, _farmer);
        ChickenUnHighlighted?.Invoke(this, _highlightedChicken);
        _highlightedChicken = null;
    }

    private bool CouldBePickedUp(Chicken chicken)
    {
        return chicken.Physical;
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

    public IEnumerable<Chicken> GetPickedUpChickens()
    {
        return _hoverLocations
            .Where(l => l.childCount > 0)
            .Select(l => l.GetChild(0))
            .Select(c => c.GetComponent<Chicken>())
            .Where(c => c != null);
    }
}
