using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Characters.MovementBehaviors
{
    public struct MovementOutput
    {
        public bool IsValid;
        public Vector2 DesiredVelocity;
        public bool ShouldJump;
    }
}
