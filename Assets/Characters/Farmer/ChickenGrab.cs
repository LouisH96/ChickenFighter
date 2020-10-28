using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class ChickenGrab : MonoBehaviour
{
    [SerializeField] private float _chickenEjectionForce = 100.0f;
    [SerializeField] private Transform[] _hoverLocations = null;
    private Chicken _chickenToPickup = null;

    void Update()
    {
        HandlePickupNewChicken();
        HandleDropChickens();
    }

    private void HandlePickupNewChicken()
    {
        if (Input.GetAxis("Action1") > 0.0f
            && _chickenToPickup)
        {
            Transform location = GetEmptyHoverLocation();
            Assert.IsNotNull(location); //if there is no location available, _chickenToPickup should be null

            //disable chicken stuff
            _chickenToPickup.EnableColliders(false);
            _chickenToPickup.EnableMovementBehavior(false);
            _chickenToPickup.EnableRigidBody(false);
            _chickenToPickup.EnableHighlight(false);

            //move chicken
            _chickenToPickup.transform.parent = location;
            _chickenToPickup.transform.localPosition = Vector3.zero;
            _chickenToPickup = null;
        }
    }

    private void HandleDropChickens()
    {
        if(Input.GetAxis("Action2") > 0.0f
            && HasChickensPickedUp())
        {
            foreach(Chicken chicken in GetPickedUpChickens())
            {
                //unparent chicken
                chicken.transform.parent = null;

                //enable chicken stuff
                chicken.EnableColliders(true);
                chicken.EnableMovementBehavior(true);
                chicken.EnableRigidBody(true);

                //add force
                chicken.GetComponent<Rigidbody>().AddForce(transform.forward * _chickenEjectionForce);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //is chicken?
        Chicken chicken = GetChickenFromCollider(other);
        if (!chicken)
            return;

        //not already picked up ?
        Assert.IsFalse(IsChickenAlreadyPickedup(chicken)); //if picked up, should no trigger this again

        //is there an empty location ?
        Transform emptyLocation = GetEmptyHoverLocation();
        if (!emptyLocation)
            return;

        //prepare for pickup
        SetChickenToPickup(chicken);
    }

    private void OnTriggerExit(Collider other)
    {
        //is chicken?
        Chicken chicken = GetChickenFromCollider(other);
        if (!chicken)
            return;

        if (chicken == _chickenToPickup)
            UnsetChickenToPickup();
    }


    //---Helperfunctions---
    private Chicken GetChickenFromCollider(Collider collider)
    {
        if (!collider.gameObject.CompareTag("Chicken"))
            return null;

        return collider.GetComponentInParent<Chicken>();
    }

    private void SetChickenToPickup(Chicken chicken)
    {
        if (!chicken)
            return;

        if (_chickenToPickup)
            UnsetChickenToPickup();

        _chickenToPickup = chicken;
        _chickenToPickup.EnableHighlight(true);
    }

    private void UnsetChickenToPickup()
    {
        if (!_chickenToPickup)
            return;

        _chickenToPickup.EnableHighlight(false);
        _chickenToPickup = null;
    }

    private Transform GetEmptyHoverLocation()
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

    private List<Chicken> GetPickedUpChickens()
    {
        return _hoverLocations
            .Where(l => l.childCount > 0)
            .Select(l => l.GetChild(0))
            .Select(c => c.GetComponent<Chicken>())
            .ToList();
    }
}
