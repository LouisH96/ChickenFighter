using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class ChickenPhysical : MonoBehaviour
{
    //---Components---
    [SerializeField] private Chicken _chicken = null;
    private List<FightBodyPart> _bodyParts = new List<FightBodyPart>();
    [SerializeField] FightBodyPart _body = null;
    private Beak2 _beak = null;
    [SerializeField] private CharacterController _characterController = null;
    [SerializeField] private Collider _generalCollider = null;
    [SerializeField] private Rigidbody _generalRigidbody = null;

    //---Variables---
    private Chicken.ChickenState _state = Chicken.ChickenState.None;
    private Transform _beforePickupTransform = null;
    private Chicken.ChickenState _beforePickupState = Chicken.ChickenState.None;
    private bool _tryToGetOutOfThrownState = false;

    void Awake()
    {
        _bodyParts = GetComponentsInChildren<FightBodyPart>().ToList();

        Assert.IsTrue(_bodyParts.TrueForAll(b => b.gameObject.layer == LayerMask.NameToLayer("FightBodyParts")), "Not all FightBodyPart were of that layer");

        _beak = GetComponentInChildren<Beak2>();
    }

    private void Start()
    {
        ChangeState(_state);
    }

    void Update()
    {
        if (_tryToGetOutOfThrownState)
            TryToGetOutOfThrownState();
    }

    private void TryToGetOutOfThrownState()
    {
        if (_generalRigidbody.velocity.sqrMagnitude < 0.1f)
        {
            _tryToGetOutOfThrownState = false;

            if (_chicken.IsInBattle())
                _chicken.ChangeState(Chicken.ChickenState.Fight);
            else
                _chicken.ChangeState(_beforePickupState);
        }
    }

    public void ChangeState(Chicken.ChickenState newState)
    {
        switch (newState)
        {
            case Chicken.ChickenState.Farm:
                SetFarmWanderState();
                break;
            case Chicken.ChickenState.Fight:
                SetFightState();
                break;
            case Chicken.ChickenState.None:
                SetNoState();
                break;
            case Chicken.ChickenState.PickedUp:
                break;
            default:
                break;
        }
    }

    private void SetNoState()
    {
        _characterController.enabled = false;
        _bodyParts.ForEach(b => b.enabled = false);
        _generalCollider.enabled = false;
        _generalRigidbody.isKinematic = true;

        _state = Chicken.ChickenState.None;
    }

    private void SetFarmWanderState()
    {
        _characterController.enabled = true;
        _bodyParts.ForEach(b => b.enabled = false);
        _generalCollider.enabled = false;
        _generalRigidbody.isKinematic = true;

        _state = Chicken.ChickenState.Farm;
    }

    public void SetPickedupState(Transform newParent)
    {
        _beforePickupTransform = newParent;
        _beforePickupState = _state;

        _characterController.enabled = false;
        _bodyParts.ForEach(b => b.enabled = false);

        _chicken.transform.parent = newParent;
        _chicken.transform.localPosition = Vector3.zero;

        _generalCollider.enabled = false;
        _generalRigidbody.isKinematic = true;

        _state = Chicken.ChickenState.PickedUp;
    }

    public void SetThrownState(Vector3 force)
    {
        _bodyParts.ForEach(b => b.enabled = false);
        _generalCollider.enabled = true;
        _generalRigidbody.isKinematic = false;

        _generalRigidbody.AddForce(force);

        _chicken.transform.parent = _beforePickupTransform;
        _beforePickupTransform = null;

        Invoke(nameof(EnableTryToGetOutOfThrownState), 0.1f);

        _state = Chicken.ChickenState.Thrown;
    }

    private void EnableTryToGetOutOfThrownState()
    {
        _tryToGetOutOfThrownState = true;
    }

    private void SetFightState()
    {
        _bodyParts.ForEach(b => b.enabled = true);
        _generalCollider.enabled = false;
        _generalRigidbody.isKinematic = true;
        _characterController.enabled = true;
        _beak.enabled = true;

        _state = Chicken.ChickenState.Fight;
    }
}
