using Pathfinding.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;

    PlayerUIController playerUIController;
    [HideInInspector] public GameObject selectedSoul;
    [HideInInspector] public float selectedSoulDistance;

    public bool[] avaiableItems;
    private int selectedItem = 0;

    [HideInInspector] public Vector2 moveInputVector;
    [HideInInspector] public bool shouldShoot;
    [HideInInspector] public Vector2 shootVector;

    [Header("Move Stats")]
    [SerializeField] float moveSpeed;
    [SerializeField] float moveAcceleration;
    [SerializeField] float moveDesacceleration;


    [Header("Spell Stats")]
    [SerializeField] float castingDistance;
    [SerializeField] GameObject spellObject;
    [SerializeField] float spellSpeed;
    [SerializeField] float spellScatter;
    [SerializeField] float spellDestructionTime;
    [SerializeField] float spellKnockback;
    float spellCooldown;

    [Header("Combat Stats")]
    public float health;
    public float maxHealth;
    public float attackDamage;
    public float attackSpeed;
    [SerializeField] float starterInvencibility;
    [SerializeField] float maxInvencibilityOnDamage;
    [SerializeField] float maxInvencibilityOnRoomEnter;

    [Header("Item Stats")]
    [SerializeField] private float potionHealing;
    [SerializeField] private float potionCooldown;
    [SerializeField] private float potionChangeDelay;
    [SerializeField] private float broomDamage;
    [SerializeField] private float broomCooldown;
    [SerializeField] private float broomHitboxSize;
    [SerializeField] private LayerMask broomLayerMask;
    [SerializeField] private GameObject broomVisual;
    [SerializeField] private float deflectedProjectileSpeed;
    [SerializeField] private float broomKnockback;
    bool bocaAberta = true;
    private float itemCooldown;

    float invencibilitySeconds;
    [HideInInspector] public bool canMove;

    void Start()
    {
        selectedSoulDistance = 69;
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
        invencibilitySeconds = starterInvencibility;
        playerUIController = GameObject.Find("PlayerUI").GetComponent<PlayerUIController>();
        UpdateUI();
        StartCoroutine(WaitForRoomTransition(1));
    }

    public void OnDoorEnter() 
    {
        canMove = false;
        invencibilitySeconds = maxInvencibilityOnRoomEnter;
        rb.velocity = Vector3.zero;
        StartCoroutine(WaitForRoomTransition(1.50f));
    }

    private IEnumerator WaitForRoomTransition(float timeWaited) 
    {
        yield return new WaitForSeconds(timeWaited);
        canMove = true;
    }

    private void Update()
    {
        if (spellCooldown > 0)
        {
            spellCooldown -= Time.deltaTime;
        }
        else if (shouldShoot && canMove) 
        {
            AudioManager.instance.PlaySFX("ATK");
            CastSpell();
        }
        if (invencibilitySeconds > 0) 
        {
            invencibilitySeconds -= Time.deltaTime;
        }
        if (itemCooldown > 0)
        {
            itemCooldown -= Time.deltaTime;
        }
        else if (!bocaAberta) 
        {
            playerUIController.CaveiraoAnim.SetTrigger("Abrir");
            bocaAberta = true;
        }
    }

    private void FixedUpdate()
    {
        if (canMove) 
        {
            //Calcula a velocidade desejada
            Vector2 targetSpeed = moveInputVector * moveSpeed;

            //Calcula a diferenca de velocidade entre a atual e a desejada
            Vector2 speedDif = targetSpeed - rb.velocity;

            //Decide a taxa de acceleracao/desaceleracao dependendo se o player quer parar completamente
            float accelRate;
            if (targetSpeed.magnitude > 0.01f)
            {
                accelRate = moveAcceleration;
            }
            else
            {
                accelRate = moveDesacceleration;
            }

            //Aplica a força
            rb.AddForce(speedDif * accelRate);
        }
    }

    public bool TakeDamage(float damage, Vector2 knockbackDirection, float knockback) 
    {
        if (invencibilitySeconds > 0) 
        {
            return false;
        }
        int sfx = (Random.Range(1, 14));
        string name = "DMG" + sfx.ToString();
        AudioManager.instance.PlaySFX(name);
        if (health <= 1)
        {
            sfx = (Random.Range(1, 3));
            name = "DEATH" + sfx.ToString();
            AudioManager.instance.PlaySFX(name);
            Die();
            return true;
        }
        health -= damage;
        if (health <= 1)
        {
            health = 1;
        }
        UpdateUI();
        invencibilitySeconds = maxInvencibilityOnDamage;
        rb.AddForce(knockback * knockbackDirection, ForceMode2D.Impulse);
        return true;
    }

    void Die() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void CastSpell() 
    {
        float xComponent = shootVector.x;
        float yComponent = shootVector.y;
        Vector2 desiredShootVector;
        Vector2 strongVector;
        if (Mathf.Abs(xComponent) > Mathf.Abs(yComponent))
        {
            //Shoot Horizontaly
            strongVector = new Vector2(xComponent, 0).normalized;
            desiredShootVector = new Vector2(strongVector.x, Random.Range(-spellScatter, spellScatter));
        }
        else
        {
            //Shoot Verticaly
            strongVector = new Vector2(0, yComponent).normalized;
            desiredShootVector = new Vector2(Random.Range(-spellScatter, spellScatter), strongVector.y);
        }
        Vector2 castingLocation = new Vector2(transform.position.x, transform.position.y) + castingDistance * strongVector;
        GameObject invokedSpell = Instantiate(spellObject, castingLocation, Quaternion.identity);
        invokedSpell.GetComponent<SpellScript>().SetUp("Enemy",attackDamage, spellSpeed * desiredShootVector,spellDestructionTime,spellKnockback);
        spellCooldown = 1 / attackSpeed;
    }

    public void Assimilate() 
    {
        if (selectedSoul != null) 
        {
            selectedSoul.GetComponent<SoulScript>().soulAssimilation();
            selectedSoul = null;
            UpdateUI();
        }
    }

    public void ChangeItemSelected() 
    {
        int i = selectedItem+1;
        i = i % (avaiableItems.Length);
        while (true) 
        {
            if (avaiableItems[i]) 
            {
                selectedItem = i;
                break;
            }
            i++;
            i = i % (avaiableItems.Length);
        }
        playerUIController.UpdateItem(selectedItem);
    }

    public void UseItem() 
    {
        if(itemCooldown > 0) 
        {
            playerUIController.CaveiraoAnim.SetTrigger("Balança");
            return;
        }
        switch (selectedItem)
        {
            case 0:
                //Unused Potion
                health += potionHealing;
                if(health > maxHealth) 
                {
                    health = maxHealth;
                }
                avaiableItems[0] = false;
                avaiableItems[1] = true;
                selectedItem = 1;
                itemCooldown = potionCooldown;
                StartCoroutine(WaitMouthClosing());
                UpdateUI();
                break;

            case 1:
                //Used Potion
                return;

            case 2:
                //Broom
                Destroy(Instantiate(broomVisual,transform.position,Quaternion.identity),0.5f);
                RaycastHit2D[] objectsHit = Physics2D.CircleCastAll(transform.position,broomHitboxSize,Vector2.zero,0,broomLayerMask);
                foreach (var item in objectsHit)
                {
                    GameObject hitObject = item.transform.gameObject;
                    string collisionTag = hitObject.tag;
                    switch (collisionTag)
                    {
                        case "Enemy":
                            EnemyBase enemy = hitObject.GetComponent<EnemyBase>();
                            if (enemy != null)
                            {
                                Vector2 directionVector = hitObject.transform.position - transform.position;
                                enemy.TakeDamage(broomDamage, directionVector,broomKnockback);
                            }
                            else
                            {
                                hitObject.GetComponent<BossController>().TakeDamage(broomDamage);
                            }
                            break;

                        case "Spell":
                            SpellScript spellScript = hitObject.GetComponent<SpellScript>();
                            Rigidbody2D spellRigidBody = hitObject.GetComponent<Rigidbody2D>();
                            spellScript.targetTag = "Enemy";
                            spellRigidBody.velocity = deflectedProjectileSpeed * (spellRigidBody.transform.position - transform.position).normalized;
                            break;
                    }
                }
                itemCooldown = broomCooldown;
                break;

            case 3:
                //Bucket
                return;
        }
        playerUIController.CaveiraoAnim.SetTrigger("Fechar");
        bocaAberta = false;
    }

    IEnumerator WaitMouthClosing() 
    {
        yield return new WaitForSeconds(potionChangeDelay);
        playerUIController.UpdateItem(selectedItem);

    }

    void UpdateUI() 
    {
        playerUIController.UpdateUI(maxHealth, health, attackDamage, attackSpeed);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position,broomHitboxSize);
    }
}
