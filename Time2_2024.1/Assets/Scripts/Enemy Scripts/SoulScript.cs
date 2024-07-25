using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoulScript : MonoBehaviour
{
    PlayerController playerReference;
    GameObject soulUI;
    TMP_Text[] soulUIText;
    CanvasGroup canvasGroupRef;

    string soulName;
    int soulStat;
    float soulAmount;
    string[] statsName = {"Health", "Attack", "Attack Speed" };
    [SerializeField] Color[] statsColor = { Color.green, Color.red, Color.blue };
    int soulRarity;
    Image soulImage;

    [SerializeField] int minHealthUp;
    [SerializeField] int maxHealthUp;
    [SerializeField] int minAttackUp;
    [SerializeField] int maxAttackUp;
    [SerializeField] int minAttackSpeedUp;
    [SerializeField] int maxAttackSpeedUp;
    [SerializeField] int attackSpeedDivider;
    [SerializeField] Sprite[] soulImageVector;
    [SerializeField] Sprite[] soulSpriteVector;

    public delegate void OnSoulAssimilation();
    public static OnSoulAssimilation onSoulAssimilation;

    private void Awake()
    {
        soulUI = GameObject.Find("SoulUI");
        soulUIText = soulUI.GetComponentsInChildren<TMP_Text>();
        soulImage = soulUI.GetComponentInChildren<Image>();
        canvasGroupRef = soulUI.GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        EnemyManager.onRoomCleared += EnableColection;
        onSoulAssimilation += DestroySoul;
    }

    private void OnDisable()
    {
        EnemyManager.onRoomCleared -= EnableColection;
        onSoulAssimilation -= DestroySoul;
    }

    void EnableColection() 
    {
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<Animator>().SetInteger("SoulType", soulStat);
    }

    public void inicialConfiguration(int stat, string name)
    {
        soulStat = stat;
        soulName = name;
        GetComponent<SpriteRenderer>().sprite = soulSpriteVector[soulStat];
        switch (soulStat)
        {
            case 0:
                soulAmount = Random.Range(minHealthUp, maxHealthUp+1);
                calculateRarity(minHealthUp, maxHealthUp);
                break;

            case 1:
                soulAmount = Random.Range(minAttackUp, maxAttackUp+1);
                calculateRarity(minAttackUp, maxAttackUp);
                break;

            case 2:
                soulAmount = Random.Range(minAttackSpeedUp, maxAttackSpeedUp);
                soulAmount /= attackSpeedDivider;
                calculateRarity(minAttackSpeedUp / attackSpeedDivider, maxAttackSpeedUp / attackSpeedDivider);
                break;

        }
    }

    private void calculateRarity(float minValue, float maxValue) 
    {
        float percentege = (soulAmount - minValue) / (maxValue - minValue); 
        switch (percentege) 
        {
            case < 0.25f:
                soulRarity = 0;
                break;

            case < 0.75f:
                soulRarity = 1;
                break;

            case > 0.75f:
                soulRarity = 2;
                break;
        }
    }

    public void soulAssimilation() 
    {
        switch (soulStat)
        {
            case 0:
                playerReference.maxHealth += soulAmount;
                playerReference.health += soulAmount;
                break;

            case 1:
                playerReference.attackDamage += soulAmount;
                break;

            case 2:
                playerReference.attackSpeed += soulAmount;
                break;
        }
        GameManager.instance.RoomCleared += 1;
        onSoulAssimilation();
    }

    public void DestroySoul() 
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerReference == null)
            {
                playerReference = collision.gameObject.GetComponent<PlayerController>();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            float distanceToPlayer = Mathf.Sqrt(Mathf.Pow(transform.position.x - playerReference.transform.position.x, 2) + Mathf.Pow(transform.position.y - playerReference.transform.position.y, 2));
            if(playerReference.selectedSoul == gameObject) 
            {
                playerReference.selectedSoulDistance = distanceToPlayer;
                updateUIPos();
                return;
            }
            if(playerReference.selectedSoulDistance > distanceToPlayer || playerReference.selectedSoul == null) 
            {
                playerReference.selectedSoul = gameObject;
                soulUIText[0].text = soulName;
                soulUIText[1].color = statsColor[soulStat];
                soulUIText[1].text = $"{statsName[soulStat]} +{soulAmount}";
                soulImage.sprite = soulImageVector[soulRarity];
                canvasGroupRef.alpha = 1;
                updateUIPos();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && playerReference.selectedSoul == gameObject)
        {
            playerReference.selectedSoul = null;
            canvasGroupRef.alpha = 0;
        }
    }

    void updateUIPos() 
    {
        Vector3 desiredPos = Camera.main.WorldToScreenPoint(transform.position) + new Vector3(0, 200, 0);
        if (desiredPos.y > Camera.main.pixelHeight - 200)
        {
            desiredPos.y = Camera.main.pixelHeight - 200;
        }
        if (desiredPos.x > Camera.main.pixelWidth - 200)
        {
            desiredPos.x = Camera.main.pixelWidth - 200;
        }
        else if (desiredPos.x < 200)
        {
            desiredPos.x = 200;
        }
        soulUI.transform.position = desiredPos;
    }
}
