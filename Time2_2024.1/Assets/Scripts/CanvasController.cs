using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    [SerializeField] GameObject transitionObject;
    public GameObject circleObject;
    GameObject player;
    Animator anim;

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

    private IEnumerator WaitForAnimation() 
    {
        circleObject.transform.position = Camera.main.WorldToScreenPoint(player.transform.position);
        transitionObject.SetActive(true);
        yield return new WaitForSeconds(1.05f);
        circleObject.transform.position = Camera.main.WorldToScreenPoint(player.transform.position);
        anim.SetBool("LeavingScene", false);
        yield return new WaitForSeconds(1);
        transitionObject.SetActive(false);
    }

    public void AnimationTransition() 
    {
        anim.SetBool("LeavingScene", true);
        StartCoroutine(WaitForAnimation());
    }
}
