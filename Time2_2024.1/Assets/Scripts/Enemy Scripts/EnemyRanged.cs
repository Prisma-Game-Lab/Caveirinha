using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : EnemyBase
{
    //descrever funcoes da classe ranged
    GameObject player;
    [SerializeField] GameObject spellObject;
    [SerializeField] float spellDamage;
    [SerializeField] float spellSpeed;
    [SerializeField] float spellScatter;
    [SerializeField] float spellDestructionTimer;
    [SerializeField] float castingDistance;
    [SerializeField] float maxCooldown;
    Animator ac;
    float cooldown;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ac = GetComponent<Animator>();
        cooldown = maxCooldown;
    }

    // Update is called once per frame
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
        ac.Play("Estagiario_Atirar");
        yield return new WaitForSeconds(0.25f);
        Vector2 directionVector = (player.transform.position - transform.position).normalized;
        Vector2 desiredShootVector;
        float xComponent = directionVector.x;
        float yComponent = directionVector.y;
        if (xComponent > 0.5f)
        {
            //Shoot Horizontaly
            xComponent = 1;
            directionVector.x = 1;
        }
        else if (xComponent < -0.5f) 
        {
            xComponent = -1;
            directionVector.x = -1;
        }
        if(yComponent > 0.5f)
        {
            //Shoot Verticaly
            yComponent = 1;
            directionVector.y = 1;
        }
        else if (yComponent < -0.5f)
        {
            //Shoot Verticaly
            yComponent = -1;
            directionVector.y = -1;
        }
        directionVector = new Vector2(xComponent,yComponent).normalized;
        desiredShootVector = new Vector2(Random.Range(-spellScatter, spellScatter) + xComponent, Random.Range(-spellScatter, spellScatter) + yComponent).normalized;
        Vector2 castingLocation = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y) + castingDistance * directionVector;
        GameObject invokedSpell = Instantiate(spellObject, castingLocation, Quaternion.identity);
        invokedSpell.GetComponent<SpellScript>().SetUp("Player", spellDamage, spellSpeed * desiredShootVector, spellDestructionTimer);
    }
}
