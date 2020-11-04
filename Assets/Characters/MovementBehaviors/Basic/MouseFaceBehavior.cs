using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFaceBehavior : FaceBehavior2
{ 
    void Update()
    {
        _target = MySessionUtils.Instance.GetWorldMousePosition();
    }
}
