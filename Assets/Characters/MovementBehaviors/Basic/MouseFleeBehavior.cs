using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFleeBehavior : FleeBehavior
{
    void Update()
    {
        Target3D = MySessionUtils.Instance.GetWorldMousePosition();
    }
}
