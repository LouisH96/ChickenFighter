using Assets.Characters.MovementBehaviors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityBehavior : MovementBehavior
{
    [SerializeField] protected List<MovementBehavior> _behaviors = new List<MovementBehavior>();

    public void Start()
    {
        _behaviors.ForEach(b => b.ShowDebugInfo = false);
    }

    public override MovementOutput HandleMovement(MovementAgent agent)
    {
        foreach (MovementBehavior behavior in _behaviors)
        {
            MovementOutput output = behavior.HandleMovement(agent);
            if (output.IsValid)
            {
                _behaviors.ForEach(b => b.ShowDebugInfo = b == behavior);
                return output;
            }
        }

        return new MovementOutput { IsValid = false };
    }
}