using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss_Laser : StateMachineBehaviour
{
    Transform[] vassouras;
    Transform player;
    public float rotation_speed = 2.5f;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        vassouras = GameObject.Find("Boss").GetComponent<BossController>().vassouras;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void rotate(Transform vassoura, Animator animator)
    {
        float angle = Mathf.Atan2(player.position.y - vassoura.position.y, player.position.x - vassoura.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));
        vassoura.rotation = Quaternion.RotateTowards(vassoura.rotation, targetRotation, rotation_speed * Time.deltaTime);

        if (targetRotation.Equals(vassoura.rotation))
        {
            animator.SetTrigger("LockOn");
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (Transform vassoura in vassouras)
        {
            rotate(vassoura, animator);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("LockOn");
    }
}
