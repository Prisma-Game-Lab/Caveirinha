using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SoulScript;

public class DoorController : MonoBehaviour
{
    //Comentario
    public Transform desiredPlayerLocation;
    public Transform desiredCameraLocation;
    public PolygonCollider2D cameraCollider;

    GameObject cameraObject;
    CinemachineConfiner2D cameraConfiner;
    BoxCollider2D doorCollider;
    SpriteRenderer doorSr;

    bool locked;

    void Start()
    {
        //Pega uma referencia a posição da camera
        cameraObject = GameObject.FindWithTag("Cinemachine");
        cameraConfiner = cameraObject.GetComponent<CinemachineConfiner2D>();
        doorCollider = gameObject.GetComponent<BoxCollider2D>();
        doorSr = gameObject.GetComponent<SpriteRenderer>();
    }

    public void toggleLock() 
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
            collision.transform.position = desiredPlayerLocation.position;
            cameraConfiner.m_BoundingShape2D = cameraCollider;
            cameraObject.transform.position = new Vector3(desiredCameraLocation.position.x,desiredCameraLocation.position.y, cameraObject.transform.position.z);
            EnemyManager enemies = desiredPlayerLocation.parent.gameObject.GetComponentInChildren<EnemyManager>(true);
            if (enemies != null)
            {
                enemies.gameObject.SetActive(true);
                enemies.LockDoors();
            }
            toggleLock();
        }
    }
}
