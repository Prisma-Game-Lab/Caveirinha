using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellScript : MonoBehaviour
{
    private Rigidbody2D rb;
    protected float damage;
    public string targetTag;
    private float knockbackAmount;

    public void SetUp(string enemyTag,float spellDamage, Vector2 speed, float destructionTimer, float knockBackStrenght) 
    {
        damage = spellDamage;
        Destroy(gameObject, destructionTimer);
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = speed;
        transform.rotation = Quaternion.Euler(0,0,Mathf.Atan2(speed.y, speed.x) * Mathf.Rad2Deg + 90);
        targetTag = enemyTag;
        knockbackAmount = knockBackStrenght;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string collisionTag = collision.gameObject.tag;
        switch (collisionTag) 
        {
            case "Enemy":
                if (targetTag == "Enemy")
                {
                    EnemyBase enemy = collision.GetComponent<EnemyBase>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(damage);
                    }
                    else
                    {
                        collision.GetComponent<BossController>().TakeDamage(damage);
                    }
                    Destroy(gameObject);
                }
                break;

            case "Player":
                if (targetTag == "Player")
                {
                    Vector2 directionVector = collision.transform.position - transform.position;
                    collision.GetComponent<PlayerController>().TakeDamage(damage,directionVector,knockbackAmount);
                    Destroy(gameObject);
                }
                break;

            case "Wall":
                Destroy(gameObject);
                break;

        }
    }
}
