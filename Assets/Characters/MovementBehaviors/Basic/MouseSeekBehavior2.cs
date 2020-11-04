using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class MouseSeekBehavior2 : SeekBehavior2
{
    void Update()
    {
        _target = MySessionUtils.Instance.MouseInWorld;
    }
}
