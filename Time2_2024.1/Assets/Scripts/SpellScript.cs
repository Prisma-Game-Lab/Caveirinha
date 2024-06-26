using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellScript : MonoBehaviour
{
    float damage;
    string targetTag;

    public void SetUp(string enemyTag,float spellDamage, Vector2 speed) 
    {
        damage = spellDamage;
        Destroy(gameObject, 10);
        GetComponent<Rigidbody2D>().velocity = speed;
        targetTag = enemyTag;
        if (targetTag == "Player") 
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string collisionTag = collision.gameObject.tag;
        switch (collisionTag) 
        {
            case "Enemy":
                if (targetTag == "Enemy")
                {
                    collision.GetComponent<EnemyBase1>().TakeDamage(damage);
                    Destroy(gameObject);
                }
                break;

            case "Player":
                if (targetTag == "Player")
                {
                    collision.GetComponent<PlayerController>().TakeDamage(damage);
                    Destroy(gameObject);
                }
                break;

            case "Wall":
                Destroy(gameObject);
                break;
        }
    }
}
