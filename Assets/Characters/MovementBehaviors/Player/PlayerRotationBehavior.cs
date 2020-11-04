using Assets.Characters.MovementBehaviors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotationBehavior : RotationBehavior
{
    public override RotationOutput HandleRotation(MovementAgent agent)
    {
        return new RotationOutput
        {
            IsValid = true,
            DesiredAngularVelocity = Input.GetAxis("MovementHorizontal") * agent.MaxAngularVelocity
        };
    }
}
