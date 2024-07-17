using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDuck : MonoBehaviour
{
    Animator anim;

    //descrever funcoes da classe healer
    [SerializeField] GameObject spellObject;
    [SerializeField] float spellDamage;
    [SerializeField] float spellSpeed;
    [SerializeField] float spellScatter;
    [SerializeField] float castingDistance;
    [SerializeField] float maxCooldown;
    [SerializeField] float spellDestructionTime;
    [SerializeField] float spellKnockback;
    float cooldown;

    void Start()
    {
        anim = GetComponent<Animator>();
        cooldown = maxCooldown;
    }

    void Update()
    {
        cooldown -= Time.deltaTime;
        if (cooldown <= 0)
        {
            anim.SetTrigger("Atirar");
            StartCoroutine(shoot());
            cooldown = maxCooldown;
            AudioManager.instance.PlaySFX("ROBSON");
        }
    }

    IEnumerator shoot()
    {
        yield return new WaitForSeconds(0.25f);
        Vector2 directionVector = new Vector2(spellSpeed, Random.Range(-spellScatter, spellScatter));
        Vector2 castingLocation = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y) + castingDistance * directionVector;
        GameObject invokedSpell = Instantiate(spellObject, castingLocation, Quaternion.identity);
        invokedSpell.GetComponent<SpellScript>().SetUp("Player", spellDamage, spellSpeed * directionVector * transform.right, spellDestructionTime, spellKnockback);
    }
}
