using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    //Comentario
    public Transform desiredPlayerLocation;
    public PolygonCollider2D cameraCollider;

    GameObject cameraObject;
    CinemachineConfiner2D cameraConfiner;
    CinemachineFramingTransposer cinemachineTransposer;
    protected BoxCollider2D doorCollider;
    protected SpriteRenderer doorSr;

    protected bool locked;

    void Start()
    {
        //Pega uma referencia a posição da camera
        cameraObject = GameObject.FindWithTag("Cinemachine");
        cameraConfiner = cameraObject.GetComponent<CinemachineConfiner2D>();
        cinemachineTransposer = cameraObject.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
        doorCollider = gameObject.GetComponent<BoxCollider2D>();
        doorSr = gameObject.GetComponent<SpriteRenderer>();
    }

    public virtual void toggleLock() 
    {
        if (locked) 
        {
            doorCollider.enabled = true;
            doorSr.color = Color.yellow;
        }
        else 
        {
            doorCollider.enabled = false;
            doorSr.color = Color.gray;
        }
        locked = !locked;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Se o player entrar na area, teleporte ele e a camera para essas posicoes
        if (collision.CompareTag("Player")) 
        {
            StartCoroutine(RoomTransition(collision.transform));
        }
    }

    private IEnumerator RoomTransition(Transform playerTransform) 
    {
        CanvasController.onSceneTransition();
        PlayerController playerScript = playerTransform.gameObject.GetComponent<PlayerController>();
        playerScript.OnDoorEnter();
        yield return new WaitForSeconds(0.85f);
        cinemachineTransposer.m_XDamping = 0;
        cinemachineTransposer.m_YDamping = 0;
        cinemachineTransposer.m_ZDamping = 0;
        playerTransform.position = desiredPlayerLocation.position;
        cameraConfiner.m_BoundingShape2D = cameraCollider;
        cameraObject.transform.position = desiredPlayerLocation.position;
        EnemyManager enemies = desiredPlayerLocation.parent.gameObject.GetComponentInChildren<EnemyManager>(true);
        if (enemies != null)
        {
            enemies.gameObject.SetActive(true);
            enemies.LockDoors();
        }
        toggleLock();
        yield return new WaitForSeconds(0.1f);
        cinemachineTransposer.m_XDamping = 1;
        cinemachineTransposer.m_YDamping = 1;
        cinemachineTransposer.m_ZDamping = 1;
    }
}
