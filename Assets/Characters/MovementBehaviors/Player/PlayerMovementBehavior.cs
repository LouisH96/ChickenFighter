using Assets.Characters.MovementBehaviors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementBehavior : MovementBehavior2
{
    public override MovementOutput HandleMovement(MovementAgent agent)
    {
        return new MovementOutput
        {
            IsValid = true,
            DesiredForwardVelocity = Input.GetAxis("MovementVertical") * agent.MaxVelocity,
            ShouldJump = Input.GetAxis("Jump") > 0.0f
        };
    }
}
