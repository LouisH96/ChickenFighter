using Assets.Characters.MovementBehaviors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityBehavior : MovementBehavior
{
    [SerializeField] protected List<MovementBehavior> _behaviors = new List<MovementBehavior>();

    public override MovementOutput HandleMovement(MovementAgent agent)
    {
        foreach(MovementBehavior behavior in _behaviors)
        {
            MovementOutput output = behavior.HandleMovement(agent);
            if (output.IsValid)
                return output;
        }

        return new MovementOutput { IsValid = false };
    }
}