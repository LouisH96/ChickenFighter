using Assets.Characters.MovementBehaviors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeBehavior : SeekBehavior
{
    public override MovementOutput HandleMovement(MovementAgent agent)
    {
        Target2D = agent.Pos2D + agent.Pos2D - Target2D;
        return base.HandleMovement(agent);
    }
}
