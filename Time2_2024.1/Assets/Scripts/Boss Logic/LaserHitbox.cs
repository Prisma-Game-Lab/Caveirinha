using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserHitbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            float damage = GameObject.Find("Boss").GetComponent<BossController>().laserDamage;
            collision.GetComponent<PlayerController>().TakeDamage(damage,Vector2.zero,0);
        }
    }
}
