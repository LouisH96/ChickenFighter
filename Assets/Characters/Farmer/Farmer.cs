using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmer : MonoBehaviour
{
    //---Components---
    [SerializeField] private ChickenGrab _chickenGrab = null;

    //---Public---
    public List<Chicken> GrabbedChickens
    {
        get
        {
            if (!_chickenGrab)
                return null;

            return _chickenGrab.GetPickedUpChickens();
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
