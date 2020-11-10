using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class ChickenPhysical : MonoBehaviour
{
    #region ---- EventArgs ---
    public class GrabbingEventArgs : CancelEventArgs
    {

    }
    public class GrabbedEventArgs : EventArgs
    {
        public Transform Position = null;
    }
    public class ThrownEventArgs : EventArgs
    {
        public Vector3 Force = Vector3.zero;
    }
    #endregion

    #region --- Events ---
    //grab/throw
    public event EventHandler<GrabbingEventArgs> Grabbing;
    public event EventHandler<GrabbedEventArgs> Grabbed;
    public event EventHandler<ThrownEventArgs> Thrown;
    public event EventHandler<Chicken> Landed;
    #endregion

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

    public bool CanGrab()
    {
        if (_state != PhysicalState.Character)
            return false;

        var args = new GrabbingEventArgs() { };
        Grabbing?.Invoke(this, args);

        return !args.Cancel;
    }

    public void Grab(Transform parent)
    {
        if (!CanGrab())
            return;

        ChangeState(PhysicalState.Kinematic);
        ChangeParent(parent, true);

        var args = new GrabbedEventArgs
        {
            Position = parent
        };
        Grabbed?.Invoke(this, args);
    }

    public void Throw(Vector3 force)
    {
        ChangeState(PhysicalState.Physics);
        ChangeParent(null, false);
        AddForce(force);

        var args = new ThrownEventArgs()
        {
            Force = force
        };
        Thrown?.Invoke(this, args);
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

        if (moveToParentOrigin)
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

            Landed?.Invoke(this, _chicken);
        }
    }

    private void EnableTryToGetOutOfPhysicsState()
    {
        _tryToGetOutOfPhysicsState = true;
    }

    #endregion

    #region --- Static Get Chicken From Collider ---

    public static Chicken GetChicken(Collider collider)
    {
        Chicken chicken = null;

        chicken = GetCharacterChickenFromCollider(collider);
        if (!chicken)
            chicken = GetPhysicsChickenFromCollider(collider);

        return chicken;
    }

    public static Chicken GetCharacterChickenFromCollider(Collider collider)
    {
        if (collider.GetType() != typeof(CharacterController))
            return null;

        return collider.gameObject.GetComponent<Chicken>();
    }

    public static Chicken GetPhysicsChickenFromCollider(Collider collider)
    {
        if (collider.CompareTag("Chicken"))
        {
            if (collider.GetType() == typeof(BoxCollider))
            {
                return collider.transform.parent.GetComponent<Chicken>();
            }
        }
        return null;
    }

    public static Chicken GetChicken(Collider collider, PhysicalState state)
    {
        switch (state)
        {
            case PhysicalState.Character:
                return GetCharacterChickenFromCollider(collider);
            case PhysicalState.Kinematic:
                return null;//todo (unused atm)
            case PhysicalState.Physics:
                return GetPhysicsChickenFromCollider(collider);
        }
        return null;
    }
    #endregion
}