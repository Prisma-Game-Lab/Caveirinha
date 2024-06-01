using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Rigidbody2D rb;

    [HideInInspector] public Vector2 moveInputVector;
    [SerializeField] float moveSpeed;
    [SerializeField] float moveAcceleration;
    [SerializeField] float moveDesacceleration;

    [HideInInspector] public bool shouldShoot;
    [HideInInspector] public Vector2 shootVector;
    [SerializeField] float castingDistance;
    [SerializeField] GameObject spellObject;
    [SerializeField] float spellSpeed;
    [SerializeField] float spellFireRate;
    [SerializeField] float spellScatter;
    float spellCooldown;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (spellCooldown > 0)
        {
            spellCooldown -= Time.deltaTime;
        }
        else if (shouldShoot) 
        {
            CastSpell();
        }
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

    void CastSpell() 
    {
        float xComponent = shootVector.x;
        float yComponent = shootVector.y;
        Vector2 desiredShootVector;
        Vector2 strongVector;
        if (Mathf.Abs(xComponent) > Mathf.Abs(yComponent))
        {
            //Shoot Horizontaly
            float direction = 1;
            if (xComponent < 0)
            {
                //Shoots left
                direction = -1;
            }
            strongVector = Vector2.right * direction;
            desiredShootVector = new Vector2(direction,Random.Range(-spellScatter, spellScatter));
        }
        else
        {
            //Shoot Verticaly
            float direction = 1;
            if (yComponent < 0)
            {
                //Shoots down
                direction = -1;
            }
            strongVector = Vector2.up * direction;
            desiredShootVector = new Vector2(Random.Range(-spellScatter, spellScatter), direction);
        }
        Vector2 castingLocation = new Vector2(gameObject.transform.position.x + castingDistance * strongVector.x, gameObject.transform.position.y + castingDistance * strongVector.y);
        GameObject invokedSpell = Instantiate(spellObject, castingLocation, Quaternion.identity);
        Rigidbody2D spellRb = invokedSpell.GetComponent<Rigidbody2D>();
        spellRb.velocity = spellSpeed * desiredShootVector;
        spellCooldown = 1 / spellFireRate;
    }
}
