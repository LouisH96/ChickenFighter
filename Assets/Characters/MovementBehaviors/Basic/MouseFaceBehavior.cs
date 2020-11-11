using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFaceBehavior : FaceBehavior
{ 
    void Update()
    {
        Target3D = MySessionUtils.Instance.GetWorldMousePosition();
    }
}
