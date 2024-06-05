using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellScript : MonoBehaviour
{
    int damage;

    public void SetUp(int spellDamage) 
    {
        damage = spellDamage;
        Destroy(gameObject, 10);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemybase>().TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
