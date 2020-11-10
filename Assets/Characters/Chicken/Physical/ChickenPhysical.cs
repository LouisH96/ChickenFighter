using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class ChickenPhysical : MonoBehaviour
{
    //--- State-Enum ---
    public enum PhysicalState
    {
        Character, Kinematic, Physics
    }

    //---Components---
    [SerializeField] private Chicken _chicken = null;

    //--- Character-State Components ---
    [SerializeField] private CharacterController _characterController = null;

    //--- Physics-State components ---
    [SerializeField] private Collider _collider = null;
    [SerializeField] private Rigidbody _rigidbody = null;

    //---Variables---
    private PhysicalState _state = PhysicalState.Character;

    //--- Physics-State Variables ---
    private bool _tryToGetOutOfPhysicsState = false;

    //--- Public Variable Accessers ---
    public PhysicalState State { get { return _state; } }

    void Awake()
    {

    }

    void Start()
    {
        EnableKinematicState(false);
        EnablePhysicsState(false);

        EnableCharacterState(true);
        _state = PhysicalState.Character;
    }

    void Update()
    {
        if (_tryToGetOutOfPhysicsState)
            TryToGetOutOfPhysicalState();
    }

    //--- Public Functions ---
    public void AddForce(Vector3 force)
    {
        if (_state != PhysicalState.Physics)
            return;

        _rigidbody.AddForce(force);
    }

    public void ChangeParent(Transform newParent, bool moveToParentOrigin)
    {
        _chicken.transform.parent = newParent;

        if(moveToParentOrigin)
            _chicken.transform.localPosition = Vector3.zero;
    }


    #region --- Change State ---
    public void ChangeState(PhysicalState newState)
    {
        if (_state == newState)
            return;

        _state = newState;

        EnablePhysicsState(newState == PhysicalState.Physics);
        EnableKinematicState(newState == PhysicalState.Kinematic);
        EnableCharacterState(newState == PhysicalState.Character);
    }

    private void EnableCharacterState(bool enable)
    {
        _characterController.enabled = enable;
    }

    private void EnableKinematicState(bool enable)
    {
        _rigidbody.isKinematic = enable;
    }

    private void EnablePhysicsState(bool enable)
    {
        _rigidbody.useGravity = enable;
        _collider.enabled = enable;

        if (enable == true)
            Invoke(nameof(EnableTryToGetOutOfPhysicsState), 0.1f);
        else
        {
            CancelInvoke(nameof(EnableTryToGetOutOfPhysicsState));
            _tryToGetOutOfPhysicsState = false;
        }
    }

    private void TryToGetOutOfPhysicalState()
    {
        Assert.IsTrue(_state == PhysicalState.Physics, "Can only try this if in physical state");

        if (_rigidbody.velocity.sqrMagnitude < 0.1f)
        {
            _tryToGetOutOfPhysicsState = false;
            ChangeState(PhysicalState.Character);
        }
    }

    private void EnableTryToGetOutOfPhysicsState()
    {
        _tryToGetOutOfPhysicsState = true;
    }

    #endregion


    //private void SetNoState()
    //{
    //    _characterController.enabled = false;
    //    _bodyParts.ForEach(b => b.enabled = false);
    //    _collider.enabled = false;
    //    _rigidbody.isKinematic = true;

    //    _state = Chicken.ChickenState.None;
    //}

    //private void SetFarmWanderState()
    //{
    //    _characterController.enabled = true;
    //    _bodyParts.ForEach(b => b.enabled = false);
    //    _collider.enabled = false;
    //    _rigidbody.isKinematic = true;

    //    _state = Chicken.ChickenState.Farm;
    //}

    //public void SetPickedupState(Transform newParent)
    //{
    //    _beforePickupTransform = newParent;
    //    _beforePickupState = _state;

    //    _characterController.enabled = false;
    //    _bodyParts.ForEach(b => b.enabled = false);

    //    _chicken.transform.parent = newParent;
    //    _chicken.transform.localPosition = Vector3.zero;

    //    _collider.enabled = false;
    //    _rigidbody.isKinematic = true;

    //    _state = Chicken.ChickenState.PickedUp;
    //}

    //public void SetThrownState(Vector3 force)
    //{
    //    _bodyParts.ForEach(b => b.enabled = false);
    //    _collider.enabled = true;
    //    _rigidbody.isKinematic = false;

    //    _rigidbody.AddForce(force);

    //    _chicken.transform.parent = _beforePickupTransform;
    //    _beforePickupTransform = null;

    //    Invoke(nameof(EnableTryToGetOutOfThrownState), 0.1f);

    //    _state = Chicken.ChickenState.Thrown;
    //}


    //private void SetFightState()
    //{
    //    _bodyParts.ForEach(b => b.enabled = true);
    //    _collider.enabled = false;
    //    _rigidbody.isKinematic = true;
    //    _characterController.enabled = true;
    //    _beak.enabled = true;

    //    _state = Chicken.ChickenState.Fight;
    //}
}
