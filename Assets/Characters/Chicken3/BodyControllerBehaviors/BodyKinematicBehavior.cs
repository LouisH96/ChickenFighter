using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class BodyKinematicBehavior : StateMachineBehaviour
{
    [SerializeField] private Chicken3 _chicken;
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
        Debug.Log("Kinematic entered");
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        Debug.Log("Kinematic update");
    }
}