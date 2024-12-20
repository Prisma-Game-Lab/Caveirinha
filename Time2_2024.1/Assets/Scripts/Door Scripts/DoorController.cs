using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] protected GameObject closedDoor;
    [SerializeField] protected GameObject openDoor;

    public GameObject DesiredRoom;
    public GameObject CurrentRoom;

    public Transform desiredPlayerLocation;
    public PolygonCollider2D cameraCollider;

    protected GameObject cameraObject;
    protected CinemachineConfiner2D cameraConfiner;
    protected CinemachineFramingTransposer cinemachineTransposer;
    protected BoxCollider2D doorCollider;

    protected bool locked;

    void Start()
    {
        //Pega uma referencia a posi��o da camera
        cameraObject = GameObject.FindWithTag("Cinemachine");
        cameraConfiner = cameraObject.GetComponent<CinemachineConfiner2D>();
        cinemachineTransposer = cameraObject.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
        doorCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    public virtual void toggleLock()
    {
        if (locked)
        {
            doorCollider.enabled = true;
            closedDoor.SetActive(false);
            openDoor.SetActive(true);
        }
        else
        {
            doorCollider.enabled = false;
            closedDoor.SetActive(true);
            openDoor.SetActive(false);
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
        DesiredRoom.SetActive(true);
        CanvasController.onSceneTransition();
        PlayerController playerScript = playerTransform.gameObject.GetComponent<PlayerController>();
        playerScript.OnDoorEnter();
        yield return new WaitForSeconds(0.85f);
        cinemachineTransposer.m_XDamping = 0;
        cinemachineTransposer.m_YDamping = 0;
        cinemachineTransposer.m_ZDamping = 0;
        cameraConfiner.m_BoundingShape2D = cameraCollider;
        playerTransform.position = desiredPlayerLocation.position;
        cameraObject.transform.position = desiredPlayerLocation.position;
        EnemyManager enemies = desiredPlayerLocation.parent.gameObject.GetComponentInChildren<EnemyManager>(true);
        if (enemies != null)
        {
            enemies.gameObject.SetActive(true);
            enemies.LockDoors();
        }
        toggleLock();
        yield return new WaitForSeconds(0.1f);
        CurrentRoom.SetActive(false);
        cinemachineTransposer.m_XDamping = 1;
        cinemachineTransposer.m_YDamping = 1;
        cinemachineTransposer.m_ZDamping = 1;
    }
}
