using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyCharacterBehavior : StateMachineBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Character entered");
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Character update");
    }
}
