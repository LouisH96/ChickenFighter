using Assets.Characters.MovementBehaviors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementAgent))]
public abstract class MovementBehavior : MonoBehaviour
{
    public bool ShowDebugInfo = true;

    abstract public MovementOutput HandleMovement(MovementAgent agent);

    void OnEnable()
    {
        ShowDebugInfo = true;
    }

    void OnDisable()
    {
        ShowDebugInfo = false;
    }
}
