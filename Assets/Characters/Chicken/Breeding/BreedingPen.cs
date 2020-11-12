using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.TerrainAPI;
using UnityEngine;
using UnityEngine.Assertions;

public class BreedingPen : MonoBehaviour
{
    //--- Components ---
    [SerializeField] private ChickenPen _pen = null;

    //--- Variables ---
    private List<Chicken> _breedableChickens = new List<Chicken>();

    //--- Unity Functions ---
    void Start()
    {
        _pen.ChickenAdded += _pen_ChickenAdded;
        _pen.ChickenRemoved += _pen_ChickenRemoved;
    }

    void Update()
    {
        Debug.Log(_breedableChickens.Count + " breedable chickens");
    }

    //--- Private Functions ---
    private void _pen_ChickenRemoved(object sender, Chicken e)
    {
        CC_Breeder breedComp = e.Breeder;

        if(breedComp)
        {
            breedComp.SetIsInBreedPen(false);
            breedComp.BreedableStateChanged -= BreedComp_BreedableStateChanged;
        }

        Assert.IsFalse(_breedableChickens.Contains(e), "chicken should not be in breedableChickens list");
    }

    private void _pen_ChickenAdded(object sender, Chicken e)
    {
        CC_Breeder breedComp = e.Breeder;

        if(breedComp)
        {
            breedComp.BreedableStateChanged += BreedComp_BreedableStateChanged;
            breedComp.SetIsInBreedPen(true);
        }
    }

    private void BreedComp_BreedableStateChanged(object sender, bool isBreedable)
    {
        Assert.IsTrue(sender is CC_Breeder, "sender should be breeder component");
        CC_Breeder breeder = (CC_Breeder)sender;

        if(isBreedable)
        {
            Assert.IsFalse(_breedableChickens.Contains(breeder.Chicken), "chicken that became breedable should not already be in breedableList");
            _breedableChickens.Add(breeder.Chicken);
        }
        else
        {
            Assert.IsTrue(_breedableChickens.Contains(breeder.Chicken), "chicken should already be in breedableList");
            _breedableChickens.Remove(breeder.Chicken);
        }
    }
}