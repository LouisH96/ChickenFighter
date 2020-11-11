using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSeekBehavior : SeekBehavior
{
    void Update()
    {
        Target3D = MySessionUtils.Instance.MouseInWorld;
    }
}
