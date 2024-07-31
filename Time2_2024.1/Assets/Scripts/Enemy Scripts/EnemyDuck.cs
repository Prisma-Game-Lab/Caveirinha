using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDuck : MonoBehaviour
{
    Animator anim;

    //descrever funcoes da classe healer
    [SerializeField] GameObject spellObject;
    [SerializeField] private float[] attackVector;
    float spellDamage;
    [SerializeField] float spellSpeed;
    [SerializeField] float spellScatter;
    [SerializeField] float castingDistance;
    [SerializeField] float minCooldown;
    [SerializeField] float maxCooldown;
    [SerializeField] float spellDestructionTime;
    [SerializeField] float spellKnockback;
    float cooldown;
    private bool shooting;

    void Start()
    {
        spellDamage = attackVector[GameManager.instance.Floor];
        anim = GetComponent<Animator>();
        cooldown = Random.Range(minCooldown*100, maxCooldown*100);
        cooldown /= 100;
    }

    void Update()
    {
        if (!shooting)
        {
            if(cooldown <= 0) 
            {
                anim.SetTrigger("Atirar");
                cooldown = Random.Range(minCooldown*100, maxCooldown * 100);
                cooldown /= 100;
                shooting = true;
            }
            else 
            {
                cooldown -= Time.deltaTime;
            }
        }
    }

    public void AnimationShoot() 
    {
        Vector2 directionVector = new Vector2(1, 0) * transform.right;
        directionVector += new Vector2(0, Random.Range(-spellScatter, spellScatter));
        directionVector = directionVector.normalized;
        Vector2 castingLocation = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y) + castingDistance * directionVector;
        GameObject invokedSpell = Instantiate(spellObject, castingLocation, Quaternion.identity);
        invokedSpell.GetComponent<SpellScript>().SetUp("Player", spellDamage, spellSpeed * directionVector, spellDestructionTime, spellKnockback);
        AudioManager.instance.PlaySFX("ROBSON");
        shooting = false;
    }

    public void death()
    {
        gameObject.GetComponent<Animator>().SetTrigger("Death");
    }
}
