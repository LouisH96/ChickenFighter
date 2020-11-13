using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BreedingPen : MonoBehaviour
{
    //--- Components ---
    [SerializeField] private ChickenPen _pen = null;

    //--- Stats ---
    [SerializeField] private float _breedDistance = 1.0f;
    [SerializeField] private float _breedTimeOutOnLeave = 5.0f;

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
        if (_breedableChickens.Count > 1)
        {
            Assert.IsTrue(_breedableChickens.TrueForAll(c => c.Breeder.IsBreedable), "unbreedable chicken in breedablelist");

            int desiredBreeds = _pen.MaxAmntOfChickens - _pen.Chickens.Count;
            TryBreed(desiredBreeds);
        }
    }

    //--- Private Functions ---
    private void _pen_ChickenRemoved(object sender, Chicken e)
    {
        CC_Breeder breedComp = e.Breeder;

        if (breedComp)
        {
            breedComp.SetIsInBreedPen(false); //will call event that removes it from _breedableChickens
            breedComp.BreedableStateChanged -= BreedComp_BreedableStateChanged;
        }

        //add some timeout to prevent them from all breeding at the same time if maxChickens was reached
        foreach (var chicken in _pen.Chickens)
        {
            CC_Breeder breeder = chicken.Breeder;
            if (!breeder)
                continue;
            breeder.AddBreedTime(Random.Range(0.0f, _breedTimeOutOnLeave));
        }

        Assert.IsFalse(_breedableChickens.Contains(e), "chicken should not be in breedableChickens list");
    }

    private void _pen_ChickenAdded(object sender, Chicken e)
    {
        CC_Breeder breedComp = e.Breeder;

        if (breedComp)
        {
            breedComp.BreedableStateChanged += BreedComp_BreedableStateChanged;
            breedComp.SetIsInBreedPen(true); //will call event that adds it to_breedableChickens
        }
    }

    private void BreedComp_BreedableStateChanged(object sender, bool isBreedable)
    {
        Assert.IsTrue(sender is CC_Breeder, "sender should be breeder component");
        CC_Breeder breeder = (CC_Breeder)sender;

        if (isBreedable)
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

    private void TryBreed(int amount)
    {
        if (amount <= 0)
            return;

        float sqrBreedDistance = _breedDistance * _breedDistance;

        for (int i = 0; i < _breedableChickens.Count - 1; i++)
        {
            Chicken a = _breedableChickens[i];
            Assert.IsTrue(a.Breeder.IsBreedable, "chicken should be breedable here");

            for (int j = i + 1; j < _breedableChickens.Count; j++)
            {
                Chicken b = _breedableChickens[j];
                Assert.IsTrue(b.Breeder.IsBreedable, "chicken should be breedable here");

                float sqrChickenDistance = (a.transform.position - b.transform.position).sqrMagnitude;
                if ((sqrChickenDistance <= sqrBreedDistance))
                {
                    a.Breeder.Breed(b);


                    if (amount > 0)
                        TryBreed(amount - 1); //recursive because breeding alters the list that is looped here
                    return;
                }
            }
        }
    }
}