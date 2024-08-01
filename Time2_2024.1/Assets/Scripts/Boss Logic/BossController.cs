using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    BlinkScript blinkScript;

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

    public Transform[] vassouras;
    GameObject[] lasers;
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

    [SerializeField] Animator animator;
    [SerializeField] Animator spriteAnimator;
    [SerializeField] GameObject portal;

    private void Start()
    {
        int rooms = GameManager.instance.RoomClearedThisFloor + GameManager.instance.RoomClearedInTotal;

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

        blinkScript = GetComponent<BlinkScript>();
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
        //StartCoroutine(blinkScript.Blink());
        if (health <= 0)
        {
            spriteAnimator.Play("FaxineiroMorte");
            animator.SetTrigger("Death");
        }
    }

    void OnDeath()
    {
        Destroy(meeleVisualInScene);
        healthUI.SetActive(false);
        portal.SetActive(true);
        GameObject robsons = GameObject.Find("Robsons");
        foreach (EnemyDuck i in robsons.GetComponentsInChildren<EnemyDuck>())
        {
            i.death();
        }
        gameObject.SetActive(false);
    }

    IEnumerator shootLaser()
    {
        playSound();
        yield return new WaitForSeconds(0);
        Vector2 targetVector;

        Transform vassoura;
        GameObject laser;
        for (int i = 0; i < vassouras.Length; i++)
        {
            vassoura = vassouras[i];
            targetVector = vassoura.up * -1;
            laser = Instantiate(laserObject, vassoura.position, vassoura.rotation);
            laser.GetComponent<SpellScript>().SetUp("Player", laserDamage, targetVector * laserSpeed, laserDestructionTime, 0);
        }
    }

    void destroyLaser()
    {
        foreach (GameObject laser in lasers)
        {
            Destroy(laser);
        }
    }

    void playSound()
    {
        int sfx = (Random.Range(1, 2));
        string name = "LASER" + sfx.ToString();
        AudioManager.instance.PlaySFX(name);
    }

    private void melleRangeCheck() 
    {
        RaycastHit2D objectHit = Physics2D.BoxCast(transform.position + meeleColliderOffset, meeleColliderSize, 0, Vector2.zero, 0, playerLayerMask);
        if (objectHit)
        {
            attacking = true;
            AudioManager.instance.PlaySFX("PVASS");
            StartCoroutine(meeleAttack());
        }
    }

    IEnumerator meeleAttack() 
    {
        meeleVisualInScene = Instantiate(meeleVisual);
        spriteAnimator.Play("Vassourada");
        meeleVisualInScene.transform.position = transform.position + meeleColliderOffset;
        meeleVisualInScene.transform.localScale = meeleColliderSize;
        yield return new WaitForSeconds(meeleAtackDelay);
        meeleVisualInScene.GetComponent<SpriteRenderer>().color = Color.red;
        AudioManager.instance.PlaySFX("BVASS");
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
