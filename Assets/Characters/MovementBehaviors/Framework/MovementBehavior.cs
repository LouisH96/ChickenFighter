using Assets.Characters.MovementBehaviors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementAgent))]
public abstract class MovementBehavior : MonoBehaviour
{
    abstract public MovementOutput HandleMovement(MovementAgent agent);
}
