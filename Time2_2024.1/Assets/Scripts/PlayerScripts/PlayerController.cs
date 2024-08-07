using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    BlinkScript blinkScript;
    CanvasController canvasController;
    PlayerUIController playerUIController;

    [HideInInspector] public GameObject selectedSoul;
    [HideInInspector] public float selectedSoulDistance;

    public bool[] avaiableItems;
    private int selectedItem = 1;

    [HideInInspector] public Vector2 moveInputVector;
    [HideInInspector] public bool shouldShoot;
    [HideInInspector] public Vector2 shootVector;

    [Header("Move Stats")]
    [SerializeField] float maxMoveSpeed;
    [SerializeField] float moveAcceleration;
    [SerializeField] float moveDesacceleration;
    [SerializeField] float broomMaxMoveSpeed;
    private float currentMaxMoveSpeed;

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
    public int potionCharges = 2;
    bool bocaAberta = true;
    private float itemCooldown;

    float invencibilitySeconds;
    [HideInInspector] public bool canMove;
    [HideInInspector] public bool gameIsPaused;

    void Start()
    {
        currentMaxMoveSpeed = maxMoveSpeed;
        selectedSoulDistance = 69;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        blinkScript = GetComponent<BlinkScript>();
        health = maxHealth;
        invencibilitySeconds = starterInvencibility;
        canvasController = GameObject.Find("Canvas").GetComponent<CanvasController>();
        playerUIController = GameObject.Find("PlayerUI").GetComponent<PlayerUIController>();
        UpdateUI();
        playerUIController.UpdateItem(selectedItem, potionCharges);
        StartCoroutine(WaitForRoomTransition(1));
        GameManager.instance.ApplyPlayerStats();
    }

    public void OnDoorEnter() 
    {
        canMove = false;
        anim.SetBool("CanMove", canMove);
        invencibilitySeconds = maxInvencibilityOnRoomEnter;
        rb.velocity = Vector3.zero;
        StartCoroutine(WaitForRoomTransition(1.50f));
    }

    private IEnumerator WaitForRoomTransition(float timeWaited) 
    {
        yield return new WaitForSeconds(timeWaited);
        canMove = true;
        anim.SetBool("CanMove", canMove);
    }

    private void Update()
    {
        if (spellCooldown > 0)
        {
            spellCooldown -= Time.deltaTime;
        }
        else if (shouldShoot && canMove && !gameIsPaused) 
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
            Vector2 targetSpeed = moveInputVector * currentMaxMoveSpeed;

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

            //Aplica a for�a
            rb.AddForce(speedDif * accelRate);
        }
    }

    public void CalculateDirection(Vector2 inputVector) 
    {
        moveInputVector = inputVector;
        anim.SetFloat("InputMagnitude", moveInputVector.sqrMagnitude);
        anim.SetFloat("Horizontal", moveInputVector.x);
        anim.SetFloat("Vertical", moveInputVector.y);
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
            health = 0;
            Die();
            return true;
        }
        health -= damage;
        if (health <= 1)
        {
            health = 1;
        }
        UpdateUI();
        StartCoroutine(blinkScript.Blink());
        invencibilitySeconds = maxInvencibilityOnDamage;
        rb.AddForce(knockback * knockbackDirection, ForceMode2D.Impulse);
        return true;
    }

    public void Die() 
    {
        UpdateUI();
        canMove = false;
        int sfx = (Random.Range(1, 3));
        string name = "DEATH" + sfx.ToString();
        AudioManager.instance.PlaySFX(name);
        anim.updateMode = AnimatorUpdateMode.UnscaledTime;
        anim.Play("Morte");
        GameManager.instance.ErasePlayerData();
        StartCoroutine(GameObject.Find("Canvas").GetComponent<CanvasController>().GameOver());
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
        if (gameIsPaused) 
        {
            return;
        }
        if (selectedSoul != null) 
        {
            selectedSoul.GetComponent<SoulScript>().soulAssimilation();
            selectedSoul = null;
            UpdateUI();
        }
    }

    public void ChangeItemSelected() 
    {
        if (!canMove || gameIsPaused)
        {
            return;
        }
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
        playerUIController.UpdateItem(selectedItem,potionCharges);
    }

    public void UseItem() 
    {
        if (!canMove || gameIsPaused) 
        {
            return;
        }
        if(itemCooldown > 0) 
        {
            AudioManager.instance.PlaySFX("COOLDOWN");
            playerUIController.CaveiraoAnim.SetTrigger("Balanca");
            return;
        }
        switch (selectedItem)
        {
            case 0:
                //Unused Potion
                if (potionCharges == 0) 
                {
                    return;
                }
                AudioManager.instance.PlaySFX("POTION");
                health += maxHealth/2;
                if(health > maxHealth) 
                {
                    health = maxHealth;
                }
                itemCooldown = potionCooldown;
                potionCharges -= 1;
                StartCoroutine(WaitMouthClosing());
                UpdateUI();
                break;

            case 1:
                //Broom
                currentMaxMoveSpeed = broomMaxMoveSpeed;
                anim.Play("Vassourada");
                int sfx = (Random.Range(1, 2));
                string name = "CVASS" + sfx.ToString();
                AudioManager.instance.PlaySFX(name);
                itemCooldown = broomCooldown;
                break;
        }
        playerUIController.CaveiraoAnim.SetTrigger("Fechar");
        bocaAberta = false;
    }

    IEnumerator WaitMouthClosing() 
    {
        yield return new WaitForSeconds(potionChangeDelay);
        playerUIController.UpdateItem(selectedItem,potionCharges);
    }

    public void UpdateUI() 
    {
        playerUIController.UpdateUI(maxHealth, health, attackDamage, attackSpeed);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position,broomHitboxSize);
    }

    public void TogglePause() 
    {
        gameIsPaused = !gameIsPaused;
        anim.SetBool("GameIsPaused", gameIsPaused);
        canvasController.PauseGame();
    }

    private void BroomAttack() 
    {
        GameObject broomVisualObject = Instantiate(broomVisual, transform);
        broomVisualObject.transform.localScale = new Vector3(broomHitboxSize * 2 / transform.localScale.x, broomHitboxSize * 2 / transform.localScale.y, 1);
        Destroy(broomVisualObject, 0.5f);
        RaycastHit2D[] objectsHit = Physics2D.CircleCastAll(transform.position, broomHitboxSize, Vector2.zero, 0, broomLayerMask);
        foreach (var item in objectsHit)
        {
            GameObject hitObject = item.transform.gameObject;
            if (hitObject.CompareTag("Enemy")) 
            {
                EnemyBase enemy = hitObject.GetComponent<EnemyBase>();
                if (enemy != null)
                {
                    enemy.TakeDamage(broomDamage);
                }
                else
                {
                    hitObject.GetComponent<BossController>().TakeDamage(broomDamage);
                }
            }
            else if (hitObject.CompareTag("Spell")) 
            {
                SpellScript spellScript = hitObject.GetComponent<SpellScript>();
                Rigidbody2D spellRigidBody = hitObject.GetComponent<Rigidbody2D>();
                spellScript.targetTag = "Enemy";
                spellRigidBody.velocity = deflectedProjectileSpeed * (spellRigidBody.transform.position - transform.position).normalized;
            }
        }
    }

    private void BroomEnd() 
    {
        currentMaxMoveSpeed = maxMoveSpeed; 
    }
}
