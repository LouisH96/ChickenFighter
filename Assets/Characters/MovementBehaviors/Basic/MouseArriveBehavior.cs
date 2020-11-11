using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseArriveBehavior : ArriveBehavior
{
    void Update()
    {
        Target3D = MySessionUtils.Instance.MouseInWorld;
    }
}
