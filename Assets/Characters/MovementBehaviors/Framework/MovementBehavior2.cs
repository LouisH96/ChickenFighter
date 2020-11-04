using Assets.Characters.MovementBehaviors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementBehavior2 : MonoBehaviour
{
    abstract public MovementOutput HandleMovement(MovementAgent agent);
}
