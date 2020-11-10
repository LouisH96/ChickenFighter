using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ChickenPen : MonoBehaviour
{
    //--- Variables ---
    List<Chicken> _chickens = new List<Chicken>();

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Chicken chicken = GetPhysicalChicken(other);

        if (!chicken)
            return;

        Assert.IsFalse(_chickens.Contains(chicken), "chicken should not be in pen");

        CC_Location locationComponent = chicken.LocationComponent;

        if (locationComponent)
            chicken.LocationComponent.OnPenEntered(this);

    }
    private void OnTriggerExit(Collider other)
    {
        Chicken chicken = GetPhysicalChicken(other);
        CC_Location locationComp = null;

        if(chicken)
        {
            locationComp = chicken.LocationComponent;
            if (locationComp)
                locationComp.OnPenExited(this);
        }
        //else
        //{
        //    Farmer farmer = GetFarmer(other);
        //    if(farmer)
        //    {
        //        foreach(Chicken grabbedChicken in farmer.GrabbedChickens)
        //        {
        //            locationComp = grabbedChicken.LocationComponent;
        //            if (locationComp)
        //                locationComp.OnPenExited(this);
        //        }
        //    }
        //}
    }

    public void EnterPen(Chicken chicken)
    {
        if (_chickens.Contains(chicken))
        {
            Assert.IsTrue(true, "chicken is already in pen");
            return;
        }

        _chickens.Add(chicken);
    }

    public void ExitPen(Chicken chicken)
    {
        if (_chickens.Contains(chicken))
            _chickens.Remove(chicken);
        else
            Assert.IsTrue(true, "cannot exit pen because chicken was not in pen");
    }

    private List<Chicken> GetChickensFromCollider(Collider other)
    {
        Chicken thrownChicken = GetPhysicalChicken(other);

        if (thrownChicken != null)
            return new List<Chicken> { thrownChicken };

        Farmer farmer = GetFarmer(other);

        if (farmer != null)
            return farmer.GrabbedChickens;

        else return null;
    }

    public Farmer GetFarmer(Collider other)
    {
        if (other.GetType() == typeof(CharacterController))
        {
            if (other.CompareTag("Player"))
            {
                return other.GetComponent<Farmer>();
            }
        }

        return null;
    }

    public Chicken GetPhysicalChicken(Collider other)
    {
        if (other.CompareTag("Chicken"))
        {
            if (other.GetType() == typeof(BoxCollider))
            {
                return other.transform.parent.GetComponent<Chicken>();
            }
        }
        return null;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    List<Chicken> enteredChickens = GetChickensFromCollider(other);

    //    if (enteredChickens != null)
    //    {
    //        foreach (var chicken in enteredChickens)
    //        {
    //            //Debug.Log("add " + chicken.name + " to battle");
    //            if (_isFarmBattle)
    //                AddChickenToNewTeam(chicken);
    //            else
    //            {
    //                if (_teams.Count > 1)
    //                    AddChickenToTeam(chicken, _teams[1]);
    //                else
    //                    AddChickenToNewTeam(chicken);
    //            }
    //        }
    //    }
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    List<Chicken> enteredChickens = GetChickensFromCollider(other);

    //    if (enteredChickens != null)
    //    {
    //        foreach (var chicken in enteredChickens)
    //        {
    //            //Debug.Log("remove " + chicken.name + " from battle");
    //            RemoveChickenOutOfBattle(chicken);
    //        }
    //    }
    //}

    //private List<Chicken> GetChickensFromCollider(Collider other)
    //{
    //    Chicken thrownChicken = IsThrownChicken(other);

    //    if (thrownChicken != null)
    //        return new List<Chicken> { thrownChicken };

    //    Farmer farmer = IsFarmer(other);

    //    if (farmer != null)
    //        return farmer.GrabbedChickens;

    //    else return null;
    //}
}
