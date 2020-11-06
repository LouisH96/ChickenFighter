using Assets.Characters.MovementBehaviors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RotationBehavior : MonoBehaviour
{
    abstract public RotationOutput HandleRotation(MovementAgent agent);
}
