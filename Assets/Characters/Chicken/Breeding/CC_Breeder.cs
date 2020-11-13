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
    [SerializeField] private float _minNewPenTimeOut = 5.0f;
    [SerializeField] private float _maxNewPenTimeOut = 15.0f;
    [SerializeField] private float _minAfterBreedTimeOut = 25.0f;
    [SerializeField] private float _maxAfterBreedTimeOut = 40.0f;
    [SerializeField] private bool _superiorRace = false;
    [SerializeField] private GameObject _bornFVXTemplate = null;

    //--- Variables ---
    private bool _isInBreedPen = false;
    private float _breedTimeOut = 0.0f;

    //--- Public Member Access ---
    public Chicken Chicken { get { return _chicken; } }
    public bool IsBreedable { get { return _isInBreedPen && _breedTimeOut <= 0.0f; } }

    public bool IsSuperiorRace { get { return _superiorRace; } }

    //--- Unity Functions ---
    private void Start()
    {
        _breedTimeOut = GetAfterBreedTimeOut();
    }

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
        if (!_superiorRace && partner.Breeder._superiorRace)
            return partner.Breeder.Breed(_chicken);

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
        AddBreedTime(GetAfterBreedTimeOut());
        partnerBreeder.AddBreedTime(partnerBreeder.GetAfterBreedTimeOut());

        //spawn & destroy vfx
        if (_bornFVXTemplate)
        {
            GameObject vfx = Instantiate(_bornFVXTemplate, transform.position, Quaternion.LookRotation(Vector3.up));
            Destroy(vfx, 2.0f);
        }

        return MakeChild(partner);
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
            AddBreedTime(GetNewPenTimeOut());

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
    private float GetAfterBreedTimeOut()
    {
        return UnityEngine.Random.Range(_minAfterBreedTimeOut, _maxAfterBreedTimeOut);
    }

    private float GetNewPenTimeOut()
    {
        return UnityEngine.Random.Range(_minNewPenTimeOut, _maxNewPenTimeOut);
    }

    private Chicken MakeChild(Chicken partner)
    {
        GameObject gameObject = Instantiate(_chicken.gameObject, transform.position, partner.transform.rotation);
        Chicken child = gameObject.GetComponent<Chicken>();

        this._chicken.Stats.BreedStats(partner.Stats, child.Stats);

        return child;
    }
}