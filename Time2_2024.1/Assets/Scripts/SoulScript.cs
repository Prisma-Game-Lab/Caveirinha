using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    Color[] statsColor = { Color.green, Color.red, Color.blue };

    public delegate void OnSoulAssimilation();
    public static OnSoulAssimilation onSoulAssimilation;

    private void Awake()
    {
        soulUI = GameObject.Find("SoulUI");
        soulUIText = soulUI.GetComponentsInChildren<TMP_Text>();
        canvasGroupRef = soulUI.GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        EnemyManager.onRoomCleared += EnableColection;
        SoulScript.onSoulAssimilation += DestroySoul;
    }

    private void OnDisable()
    {
        EnemyManager.onRoomCleared -= EnableColection;
        SoulScript.onSoulAssimilation -= DestroySoul;
    }

    void EnableColection() 
    {
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<SpriteRenderer>().color = Color.blue;
    }

    public void inicialConfiguration(int stat, string name)
    {
        soulStat = stat;
        soulName = name;
        switch (soulStat)
        {
            case 0:
                soulAmount = Random.Range(15,51);
                break;

            case 1:
                soulAmount = Random.Range(15, 31);
                break;

            case 2:
                soulAmount = Random.Range(1, 5);
                soulAmount /= 2;
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
            playerReference.selectedSoul = gameObject;
            updateUIPos();
            soulUIText[0].text = soulName;
            soulUIText[1].color = statsColor[soulStat];
            soulUIText[1].text = $"{statsName[soulStat]} +{soulAmount}";
            canvasGroupRef.alpha = 1;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            updateUIPos();
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
        if (desiredPos.y > Camera.main.pixelHeight - 125)
        {
            desiredPos.y = Camera.main.pixelHeight - 125;
        }
        if (desiredPos.x > Camera.main.pixelWidth - 160)
        {
            desiredPos.x = Camera.main.pixelWidth - 160;
        }
        else if (desiredPos.x < 160)
        {
            desiredPos.x = 160;
        }
        soulUI.transform.position = desiredPos;
    }
}
