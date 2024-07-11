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
    private float baseHealth;
    [SerializeField]
    public float baseLaser;

    public float health;
    public float laserDamage;

    [SerializeField]
    GameObject soulObject;
    [SerializeField]
    int soulType;
    [SerializeField] 
    GameObject laserObject;
    [SerializeField]
    float laserSpeed;
    //So pro setup
    [SerializeField]
    float laserDestructionTime;

    GameObject laser;

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
        //GameObject soul = Instantiate(soulObject, transform.position, Quaternion.identity);
        //soul.GetComponent<SoulScript>().inicialConfiguration(soulType, enemyName);
        Destroy(gameObject);
    }

    IEnumerator shootLaser()
    {
        yield return new WaitForSeconds(0);
        Transform vassoura = GameObject.Find("Vassoura_Laser").transform;

        Vector2 targetVector = vassoura.up * -1;
        laser = Instantiate(laserObject, vassoura.position, vassoura.rotation);
        laser.GetComponent<SpellScript>().SetUp("Player", laserDamage, targetVector * laserSpeed,laserDestructionTime, 0);
    }

    void destroyLaser()
    {
        Destroy(laser);
    }

    void playSound(string sfx)
    {
        AudioManager.instance.PlaySFX(sfx);
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
