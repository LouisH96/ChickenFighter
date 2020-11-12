using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.ProGrids;

public class CC_Breeder : MonoBehaviour
{
    //--- Events ---
    public event EventHandler<bool> BreedableStateChanged;

    //--- Components ---
    [SerializeField] private Chicken _chicken = null;

    //--- Stats ---
    [SerializeField] private float _timeOutOnPenEnter = 15.0f;
    [SerializeField] private float _timeOutAfterBreed = 30.0f;

    //--- Variables ---
    private bool _isInBreedPen = false;
    private float _breedTimeOut = 0.0f;

    //--- Public Member Access ---
    public Chicken Chicken { get { return _chicken; } }
    public bool IsBreedable { get { return _isInBreedPen && _breedTimeOut <= 0.0f; } }

    //--- Unity Functions ---
    private void Update()
    {
        if (_breedTimeOut > 0.0f)
        {
            _breedTimeOut -= Time.deltaTime;
            if (IsBreedable)
                BreedableStateChanged?.Invoke(this, true);
        }
    }

    //--- Public Functions ---
    public Chicken Breed(Chicken partner)
    {
        CC_Breeder partnerBreeder = partner.Breeder;

        //check if code that calls this, calls it on the right time/state
        if (!IsBreedable)
        {
            Debug.LogWarning("chicken should not breed if it is not breedable");
            return null;
        }
        if (!partnerBreeder)
        {
            Debug.LogWarning("partner has no breed component");
            return null;
        }
        if (!partnerBreeder.IsBreedable)
        {
            Debug.LogWarning("partnerchicken should not breed if it is not breedable");
            return null;
        }

        //increase breedTimeOut
        AddBreedTime(_timeOutAfterBreed);
        partnerBreeder.AddBreedTime(partnerBreeder._timeOutAfterBreed);

        //do breeding
        GameObject child = Instantiate(_chicken.gameObject);

        //return child
        return child.GetComponent<Chicken>();
    }
    public void SetIsInBreedPen(bool isInBreedPen)
    {
        if (_isInBreedPen == isInBreedPen)
        {
            Debug.LogWarning("Should not be called when already in this state");
            return;
        }

        //change state
        if (isInBreedPen)
            AddBreedTime(_timeOutOnPenEnter);

        //store state
        bool wasBreedable = IsBreedable;

        //change state
        _isInBreedPen = isInBreedPen;

        //raise event on state changed
        if (wasBreedable != IsBreedable)
            BreedableStateChanged?.Invoke(this, IsBreedable);
    }
    public void AddBreedTime(float amount)
    {
        //store current state
        bool wasBreedable = IsBreedable;

        //change state
        if (_breedTimeOut < 0.0f)
            _breedTimeOut = amount;
        else
            _breedTimeOut += amount;

        //invoke event on state change
        if (IsBreedable != wasBreedable)
            BreedableStateChanged?.Invoke(this, IsBreedable);
    }

    //--- Private Functions ---
}