using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Laser : StateMachineBehaviour
{
    Transform vassoura;
    Transform mira;
    Transform player;
    public float rotation_speed = 2.5f;


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
        float angle = Mathf.Atan2(player.position.y - vassoura.position.y, player.position.x - vassoura.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle+90));
        vassoura.rotation = Quaternion.RotateTowards(vassoura.rotation, targetRotation, rotation_speed * Time.deltaTime);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
}
