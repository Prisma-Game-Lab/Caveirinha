using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Unity.UI;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    [SerializeField]
    private string enemyName;
    [SerializeField]
    private string enemyClass;
    [SerializeField]
    private float baseHealth;
    [SerializeField]
    public float baseLaser;

    public float health;
    public float laserDamage;

    [SerializeField] 
    GameObject laserObject;
    [SerializeField]
    float laserSpeed;
    [SerializeField]
    float laserDestructionTime;

    GameObject laserL, laserR;
    GameObject meeleVisualInScene;

    [SerializeField]
    private GameObject healthUI;
    private Slider slider;

    bool attacking;
    [SerializeField] Vector3 meeleColliderOffset;
    [SerializeField] Vector2 meeleColliderSize;
    [SerializeField] LayerMask playerLayerMask;
    [SerializeField] GameObject meeleVisual;
    [SerializeField] float meeleAtackDelay;
    [SerializeField] float meeleAttackCooldown;
    [SerializeField] float meeleAttackDamage;
    [SerializeField] float meeleKnockbackStrenght;

    private void Start()
    {
        int rooms = GameManager.instance.RoomCleared;

        health = baseHealth;
        laserDamage = baseLaser;
        if (rooms > 3) 
        {
            health = baseHealth * Mathf.Log(rooms, 2.3f);
            laserDamage = baseLaser * Mathf.Pow(1.03f, rooms);
        }
        healthUI.SetActive(true);
        slider = healthUI.GetComponent<Slider>();
        slider.maxValue = health;
        slider.value = health;
    }

    private void FixedUpdate()
    {
        if (!attacking) 
        {
            melleRangeCheck();
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        slider.value = health;
        if (health <= 0)
        {
            OnDeath();
        }
    }

    void OnDeath()
    {
        Destroy(meeleVisualInScene);
        Destroy(gameObject);
        healthUI.SetActive(false);
    }

    IEnumerator shootLaser()
    {
        yield return new WaitForSeconds(0);
        Transform vassouraL = GameObject.Find("Vassoura_Laser L").transform;
        Transform vassouraR = GameObject.Find("Vassoura_Laser R").transform;

        Vector2 targetVector = vassouraL.up * -1;
        laserL = Instantiate(laserObject, vassouraL.position, vassouraL.rotation);
        laserL.GetComponent<SpellScript>().SetUp("Player", laserDamage, targetVector * laserSpeed,laserDestructionTime, 0);

        targetVector = vassouraR.up * -1;
        laserR = Instantiate(laserObject, vassouraR.position, vassouraR.rotation);
        laserR.GetComponent<SpellScript>().SetUp("Player", laserDamage, targetVector * laserSpeed, laserDestructionTime, 0);
    }

    void destroyLaser()
    {
        Destroy(laserL);
        Destroy(laserR);
    }

    void playSound(string sfx)
    {
        AudioManager.instance.PlaySFX(sfx);
    }

    private void melleRangeCheck() 
    {
        RaycastHit2D objectHit = Physics2D.BoxCast(transform.position + meeleColliderOffset, meeleColliderSize, 0, Vector2.zero, 0, playerLayerMask);
        if (objectHit)
        {
            attacking = true;
            StartCoroutine(meeleAttack());
        }
    }

    IEnumerator meeleAttack() 
    {
        meeleVisualInScene = Instantiate(meeleVisual);
        meeleVisualInScene.transform.position = transform.position + meeleColliderOffset;
        meeleVisualInScene.transform.localScale = meeleColliderSize;
        yield return new WaitForSeconds(meeleAtackDelay);
        meeleVisualInScene.GetComponent<SpriteRenderer>().color = Color.red;
        RaycastHit2D objectHit = Physics2D.BoxCast(transform.position + meeleColliderOffset, meeleColliderSize, 0, Vector2.zero, 0, playerLayerMask);
        if(objectHit) 
        {
            Vector2 directionVector = objectHit.transform.position - transform.position;
            objectHit.transform.gameObject.GetComponent<PlayerController>().TakeDamage(meeleAttackDamage, directionVector, meeleKnockbackStrenght);
        }
        Destroy(meeleVisualInScene,0.5f);
        yield return new WaitForSeconds(meeleAttackCooldown);
        attacking = false;
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Vector2 directionVector = collision.transform.position - transform.position;
            collision.GetComponent<PlayerController>().TakeDamage(contactDamage);
            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();
            playerRb.velocity = Vector2.zero;
            playerRb.AddForce(knockbackStrength * directionVector.normalized, ForceMode2D.Impulse);
        }
    }*/

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position + meeleColliderOffset, meeleColliderSize);
    }
}
