using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMage : EnemyBase
{
    //descrever funcoes da classe healer
    GameObject player;
    [SerializeField] GameObject spellObject;
    [SerializeField] float spellDamage;
    [SerializeField] float spellSpeed;
    [SerializeField] float spellScatter;
    [SerializeField] float castingDistance;
    [SerializeField] float maxCooldown;
    float cooldown;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cooldown = maxCooldown;
    }

    void Update()
    {
        cooldown -= Time.deltaTime;
        if (cooldown <= 0)
        {
            StartCoroutine(shoot());
            cooldown = maxCooldown;
        }
    }

    IEnumerator shoot()
    {
        yield return new WaitForSeconds(0.25f);
        Vector2 directionVector = (player.transform.position - transform.position).normalized;
        Vector2 desiredShootVector;
        float xComponent = directionVector.x;
        float yComponent = directionVector.y;        
        directionVector = new Vector2(xComponent, yComponent).normalized;
        desiredShootVector = new Vector2(Random.Range(-spellScatter, spellScatter) + xComponent, Random.Range(-spellScatter, spellScatter) + yComponent).normalized;
        Vector2 castingLocation = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y) + castingDistance * directionVector;
        GameObject invokedSpell = Instantiate(spellObject, castingLocation, Quaternion.identity);
        invokedSpell.GetComponent<SpellScript>().SetUp("Player", spellDamage, spellSpeed * desiredShootVector);
    }
}
