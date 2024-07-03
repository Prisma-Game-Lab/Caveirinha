using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Comentario
    Rigidbody2D rb;

    [HideInInspector] public Vector2 moveInputVector;
    [SerializeField] float moveSpeed;
    [SerializeField] float moveAcceleration;
    [SerializeField] float moveDesacceleration;

    [HideInInspector] public bool shouldShoot;
    [HideInInspector] public Vector2 shootVector;
    [SerializeField] float castingDistance;
    [SerializeField] GameObject spellObject;
    [SerializeField] float spellSpeed;
    [SerializeField] float spellScatter;
    float spellCooldown;
    [SerializeField] GameObject playerUiPrefab;

    [SerializeField] float maxInvencibility;
    public float invencibilitySeconds;

    public GameObject selectedSoul;
    Slider healthUI;

    [Header("Stats")]
    public float maxHealth;
    public float attackDamage;
    public float attackSpeed;
    public float health;

    public bool canMove;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
        GameObject PlayerUIInScene = GameObject.Find("PlayerUI");
        if (PlayerUIInScene == null) 
        {
            GameObject canvas = GameObject.Find("Canvas");
            PlayerUIInScene = Instantiate(playerUiPrefab, canvas.transform);
        }
        healthUI = PlayerUIInScene.GetComponentInChildren<Slider>();
        StartCoroutine(WaitForRoomTransition(1));
    }

    public void OnDoorEnter() 
    {
        canMove = false;
        invencibilitySeconds = 2;
        rb.velocity = Vector3.zero;
        StartCoroutine(WaitForRoomTransition(1.75f));
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
        else if (shouldShoot) 
        {
            CastSpell();
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
                accelRate = moveAcceleration;
            }

            //Aplica a força
            rb.AddForce(speedDif * accelRate);

            invencibilitySeconds -= Time.deltaTime;
        }
    }

    public void TakeDamage(float damage) 
    {
        if (invencibilitySeconds < 0) 
        {
            int sfx = (Random.Range(1, 25));
            string name = "DMG" + sfx.ToString();
            AudioManager.instance.PlaySFX(name);
            if (health <= 1)
            {
                Die();
                return;
            }
            health -= damage;
            if (health <= 1)
            {
                health = 1;
            }
            UpdateUI();
            invencibilitySeconds = maxInvencibility;
        }
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
        Vector2 castingLocation = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y) + castingDistance * strongVector;
        GameObject invokedSpell = Instantiate(spellObject, castingLocation, Quaternion.identity);
        invokedSpell.GetComponent<SpellScript>().SetUp("Enemy",attackDamage, spellSpeed * desiredShootVector);
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

    void UpdateUI() 
    {
        healthUI.maxValue = maxHealth;
        healthUI.value = health;
    }
}
