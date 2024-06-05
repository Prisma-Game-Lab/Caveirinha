using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemybase : MonoBehaviour
{
    public EnemyInfo enemy;
    SpriteRenderer sr;
    string enemyName;
    string enemyClass;
    int health;
    int maxHealth;
    int AttackDamage;

    private void Awake()
    {
        enemyName = enemy.enemyName;
        enemyClass = enemy.enemyClass;
        AttackDamage = enemy.enemyAttack;
        maxHealth = enemy.enemyMaxHealth;
        health = maxHealth;
        sr = GetComponent<SpriteRenderer>();
        sr.color = Color.magenta;
    }

    public void TakeDamage(int damage) 
    {
        health -= damage;
        if(health <= 0) 
        {
            OnDeath();
        }
    }

    void OnDeath() 
    {
        Destroy(gameObject);
    }
}
