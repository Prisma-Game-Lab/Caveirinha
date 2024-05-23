using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    Rigidbody2D rb;

    [HideInInspector] public Vector2 moveInputVector;
    [SerializeField] float moveSpeed;
    [SerializeField] float moveAcceleration;
    [SerializeField] float moveDesacceleration;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        //Calcula a velocidade desejada
        Vector2 targetSpeed = moveInputVector * moveSpeed;

        //Calcula a diferenca de velocidade entre a atual e a desejada
        Vector2 speedDif = targetSpeed - rb.velocity;

        //Decide a taxa de acceleracao/desaceleracao dependendo se o player quer parar completamente
        float accelRate;
        if (targetSpeed.magnitude > 0.01f) 
        {
            accelRate = moveAcceleration;
        }
        else 
        {
            accelRate = moveAcceleration;
        }

        //Aplica a força
        rb.AddForce(speedDif * accelRate);
    }
}
