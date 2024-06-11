using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : EnemyBase1
{
    //descrever funcoes da classe ranged
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

    // Update is called once per frame
    void Update()
    {
        cooldown -= Time.deltaTime;
        if (cooldown <= 0)
        {
            shoot();
            cooldown = maxCooldown;
        }
    }

    void shoot()
    {
        Vector2 directionVector = player.transform.position - transform.position;
        Vector2 desiredShootVector;
        float xComponent = directionVector.x;
        float yComponent = directionVector.y;
        if (Mathf.Abs(xComponent) > Mathf.Abs(yComponent))
        {
            //Shoot Horizontaly
            directionVector = new Vector2(xComponent, 0).normalized;
            desiredShootVector = new Vector2(directionVector.x, Random.Range(-spellScatter, spellScatter));
        }
        else
        {
            //Shoot Verticaly
            directionVector = new Vector2(0, yComponent).normalized;
            desiredShootVector = new Vector2(Random.Range(-spellScatter, spellScatter), directionVector.y);
        }
        Vector2 castingLocation = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y) + castingDistance * directionVector;
        GameObject invokedSpell = Instantiate(spellObject, castingLocation, Quaternion.identity);
        invokedSpell.GetComponent<SpellScript>().SetUp("Player", spellDamage, spellSpeed * desiredShootVector);
    }
}
