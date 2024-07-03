using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField]
    private string enemyName;
    [SerializeField]
    private string enemyClass;
    [SerializeField]
    private float health;
    [SerializeField]
    private float enemyAttack;
    [SerializeField]
    private float contactDamage;
    [SerializeField]
    private float knockbackStrength;
    [SerializeField]
    GameObject soulObject;
    [SerializeField]
    int soulType;
    [SerializeField] 
    GameObject laserObject;
    [SerializeField]
    float laserSpeed;

    GameObject laser;

    private void Awake()
    {

    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            OnDeath();
        }
    }

    void OnDeath()
    {
        GameObject soul = Instantiate(soulObject, transform.position, Quaternion.identity);
        soul.GetComponent<SoulScript>().inicialConfiguration(soulType, enemyName);
        Destroy(gameObject);
    }

    IEnumerator shootLaser()
    {
        float spellDamage = 1;
        yield return new WaitForSeconds(0);
        Transform vassoura = GameObject.Find("Vassoura_Laser").transform;

        Vector2 targetVector = vassoura.up * -1;
        laser = Instantiate(laserObject, vassoura.position, vassoura.rotation);
        laser.GetComponent<SpellScript>().SetUp("Player", spellDamage, targetVector * laserSpeed);
    }

    void destroyLaser()
    {
        Destroy(laser);
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
}
