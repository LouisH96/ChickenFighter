using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Chicken : MonoBehaviour
{
    //---Components---
    [SerializeField] private Renderer _highlightRenderer = null;

    private MovementBehavior _movementBehavior = null;
    private CharacterController _characterController = null;

    private Rigidbody _rigidBody = null;
    private BoxCollider _boxCollider = null;


    //---Variables---

    /*
     * used to delay the rigidbody velocity check
     * because it is 0 in the frame the force is applied
     */
    private bool _tryToGoCharacterMode = false;

    private void Awake()
    {
        _movementBehavior = GetComponent<MovementBehavior>();
        _characterController = GetComponent<CharacterController>();

        _rigidBody = GetComponent<Rigidbody>();
        _boxCollider = GetComponent<BoxCollider>();

        if (_movementBehavior)
        {
            EnableCharacterMode(true);
            EnablePhysicsMode(false);
        }
        else
        {
            EnablePhysicsMode(true);
        }
    }

    private void Update()
    {
        if (IsInPhysicsMode()
            && _tryToGoCharacterMode
            && _rigidBody.velocity.sqrMagnitude < 0.1f)
        {
            EnablePhysicsMode(false);
            EnableCharacterMode(true);
            _tryToGoCharacterMode = false;
        }
    }

    public void Pickup()
    {
        EnableCharacterMode(false);
        EnablePhysicsMode(false);
    }

    public void Throw(Vector3 force)
    {
        if (_rigidBody)
        {
            EnableCharacterMode(false);
            EnablePhysicsMode(true);
            
            _rigidBody.AddForce(force);
            Invoke(nameof(EnableTryToGoCharacterMode), 0.1f);
        }
        else
        {
            EnableCharacterMode(true);
        }
    }

    private void EnableTryToGoCharacterMode()
    {
        _tryToGoCharacterMode = true;
    }

    public void EnableHighlight(bool enable)
    {
        if (!_highlightRenderer)
            return;

        _highlightRenderer.enabled = enable;
    }

    private void EnableCharacterMode(bool enable)
    {
        if (_movementBehavior)
            _movementBehavior.enabled = enable;

        if (_characterController)
            _characterController.enabled = enable;
    }

    private void EnablePhysicsMode(bool enable)
    {
        if (_rigidBody)
        {
            _rigidBody.velocity = Vector3.zero;
            _rigidBody.useGravity = enable;
            _rigidBody.isKinematic = !enable;
        }

        if (_boxCollider)
            _boxCollider.enabled = enable;
    }

    private bool IsInPhysicsMode()
    {
        return _rigidBody && !_rigidBody.isKinematic;
    }
}