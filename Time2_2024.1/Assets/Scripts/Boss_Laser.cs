using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Laser : StateMachineBehaviour
{
    Transform vassoura;
    Transform mira;
    Transform player;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        vassoura = GameObject.Find("Vassoura_Laser").transform;
        mira = GameObject.Find("Aim").transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        vassoura.LookAt(player, animator.transform.up);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
}
