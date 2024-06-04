using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    //Comentario
    public Transform desiredPlayerLocation;
    public Transform desiredCameraLocation;

    Transform cameraTransform;

    void Start()
    {
        //Pega uma referencia a posição da camera
        cameraTransform = GameObject.FindWithTag("MainCamera").transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Se o player entrar na area, teleporte ele e a camera para essas posicoes
        if (collision.CompareTag("Player")) 
        {
            collision.transform.position = desiredPlayerLocation.position;
            cameraTransform.position = new Vector3(desiredCameraLocation.position.x,desiredCameraLocation.position.y, cameraTransform.position.z);
        }
    }
}
