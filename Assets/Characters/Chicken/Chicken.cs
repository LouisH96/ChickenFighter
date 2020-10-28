using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : MonoBehaviour
{
    [SerializeField] private Renderer _highlightRenderer = null;

    public void EnableMovementBehavior(bool enable)
    {
        MonoBehaviour movement = GetComponent<PositionMovementBehavior>();

        if (movement)
            movement.enabled = enable;
    }

    public void EnableColliders(bool enable)
    {
        foreach (Collider collider in GetComponentsInChildren<Collider>())
            collider.enabled = enable;
    }

    public void EnableHighlight(bool enable)
    {
        if (!_highlightRenderer)
            return;

        _highlightRenderer.enabled = enable;
    }

    public void EnableRigidBody(bool enable)
    {
        Rigidbody rigidBody = GetComponent<Rigidbody>();
        rigidBody.useGravity = enable;
        rigidBody.isKinematic = !enable;
    }
}
