using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    [SerializeField] GameObject transitionObject;
    public GameObject circleObject;
    GameObject player;
    Animator anim;
    bool isOnTransition;

    public delegate void OnSceneTransition();
    public static OnSceneTransition onSceneTransition;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        transitionObject.SetActive(true);
        player = GameObject.Find("Player");
    }

    private void OnEnable()
    {
        onSceneTransition += AnimationTransition;
    }

    private void OnDisable()
    {
        onSceneTransition -= AnimationTransition;
    }

    private void FixedUpdate()
    {
        if (isOnTransition) 
        {
            circleObject.transform.position = Camera.main.WorldToScreenPoint(player.transform.position);
        }
    }

    private IEnumerator WaitForAnimation() 
    {
        transitionObject.SetActive(true);
        isOnTransition = true;
        yield return new WaitForSeconds(2);
        isOnTransition = false;
        transitionObject.SetActive(false);
    }

    public void AnimationTransition() 
    {
        anim.SetTrigger("LeavingScene");
        StartCoroutine(WaitForAnimation());
    }
}
