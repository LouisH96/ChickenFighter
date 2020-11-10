using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

//ChickenComponent_Location
public class CC_Location : MonoBehaviour
{
    //--- Components ---


    //--- Variables ---
    private ChickenPen _pen = null;
    private Farmer _farmer = null;


    //temp
    public ChickenPen Pen { get => _pen; set => _pen = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPenEntered(ChickenPen enteredPen)
    {
        Assert.IsNull(_pen, "there should be no pen");

        _pen = enteredPen;
        Debug.Log("Pen set");
    }

    public void OnPenExited(ChickenPen exitedPen)
    {
        Assert.AreEqual(_pen, exitedPen, "should not be able to leave another pen then the one it is in");

        _pen = null;
        Debug.Log("Pen unset");

    }

    public void SetGrabbed( )
    {
        _pen = null;
        Debug.Log("Chicken grabbed");
    }
}
