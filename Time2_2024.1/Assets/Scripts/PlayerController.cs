using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Comentario
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
    [SerializeField] float spellDamage;
    float spellCooldown;

    [SerializeField] float maxHealth;
    [SerializeField] Slider healthUI;
    float health;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
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

    public void TakeDamage(float damage) 
    {
        if (health <= 1) 
        {
            Die();
            return;
        }
        health -= damage;
        if (health <= 1)
        {
            health = 1;
        }
        UpdateUI();
    }

    void Die() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
            strongVector = new Vector2(xComponent, 0).normalized;
            desiredShootVector = new Vector2(strongVector.x, Random.Range(-spellScatter, spellScatter));
        }
        else
        {
            //Shoot Verticaly
            strongVector = new Vector2(0, yComponent).normalized;
            desiredShootVector = new Vector2(Random.Range(-spellScatter, spellScatter), strongVector.y);
        }
        Vector2 castingLocation = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y) + castingDistance * strongVector;
        GameObject invokedSpell = Instantiate(spellObject, castingLocation, Quaternion.identity);
        invokedSpell.GetComponent<SpellScript>().SetUp("Enemy",spellDamage, spellSpeed * desiredShootVector);
        spellCooldown = 1 / spellFireRate;
    }

    void UpdateUI() 
    {
        healthUI.value = health;
    }
}
