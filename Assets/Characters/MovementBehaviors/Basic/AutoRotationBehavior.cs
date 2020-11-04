using Assets.Characters.MovementBehaviors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotationBehavior : FaceBehavior2
{
    public override RotationOutput HandleRotation(MovementAgent agent)
    {
        return base.HandleRotation(agent);
    }
}
