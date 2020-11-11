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
    //--- State-Enum ---
    public enum PhysicalState
    {
        Begin, Character, Kinematic, Physics
    }

    //---Components---
    [SerializeField] private Chicken _chicken = null;
    [SerializeField] private CharacterController _characterController = null;
    [SerializeField] private Collider _collider = null;
    [SerializeField] private Rigidbody _rigidbody = null;

    void Awake()
    {

    }

    void Start()
    {
        if (_state == PhysicalState.Begin)
            ChangeState(PhysicalState.Character);
    }

    void Update()
    {
        if (_tryToGetOutOfPhysicsState)
            TryToGetOutOfPhysicalState();
    }

    #region --- Grab/Throw ---
    public class GrabbingEventArgs : CancelEventArgs { }
    public class GrabbedEventArgs : EventArgs { public Transform Position = null; }
    public class ThrownEventArgs : EventArgs { public Vector3 Force = Vector3.zero; }

    public event EventHandler<GrabbingEventArgs> Grabbing;
    public event EventHandler<GrabbedEventArgs> Grabbed;
    public event EventHandler<ThrownEventArgs> Thrown;
    public event EventHandler<Chicken> Landed;

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

    #endregion

    #region --- General Functions ---
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
    #endregion

    #region --- State ---
    //--- Event Args ---
    public class StateChangedEventArgs : EventArgs
    {
        public PhysicalState OldState;
        public PhysicalState NewState;
    }

    //--- Events ---
    public event EventHandler<StateChangedEventArgs> StateChanged;

    //--- Private variables ---
    private PhysicalState _state = PhysicalState.Begin;
    private bool _tryToGetOutOfPhysicsState = false;

    //--- Public variable Access ---
    public PhysicalState State { get { return _state; } }

    //--- Public Functions ---
    public void ChangeState(PhysicalState newState)
    {
        if (_state == newState)
            return;

        var eventArgs = new StateChangedEventArgs()
        {
            OldState = _state,
            NewState = newState
        };

        _state = newState;

        EnablePhysicsState(newState == PhysicalState.Physics);
        EnableKinematicState(newState == PhysicalState.Kinematic);
        EnableCharacterState(newState == PhysicalState.Character);

        StateChanged?.Invoke(this, eventArgs);
    }

    //--- Private Functions ---
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