using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Comentario
    public static GameManager instance { get; private set; }

    public int Floor = 0;
    public int RoomClearedThisFloor = 0;
    public int RoomClearedInTotal;

    float playerMaxHealth;
    float playerAttackDamage;
    float playerAttackSpeed;

    float playerStarterMaxHealth;
    float playerStarterAttackDamage;
    float playerStarterAttackSpeed;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SavePlayerStats(GameObject player) 
    {
        Floor++;
        PlayerController playerScript = player.GetComponent<PlayerController>();
        playerMaxHealth = playerScript.maxHealth;
        playerAttackDamage = playerScript.attackDamage;
        playerAttackSpeed = playerScript.attackSpeed;
        RoomClearedInTotal = RoomClearedThisFloor;
        RoomClearedThisFloor = 0;
    }

    public void ApplyPlayerStats() 
    {
        if(playerMaxHealth == 0) 
        {
            return;
        }
        GameObject player = GameObject.Find("Player");
        PlayerController playerScript = player.GetComponent<PlayerController>();
        playerScript.maxHealth = playerMaxHealth;
        playerScript.health = playerMaxHealth;
        playerScript.attackDamage = playerAttackDamage;
        playerScript.attackSpeed = playerAttackSpeed;
        playerScript.UpdateUI();
    }

    public void ErasePlayerData() 
    {
        Floor = 0;
        RoomClearedThisFloor = 0;
        RoomClearedInTotal = 0;
        playerMaxHealth = playerStarterMaxHealth;
        playerAttackDamage = playerStarterAttackDamage;
        playerAttackSpeed = playerStarterAttackSpeed;
    }

    public void SetDificulty(float health, float attackDamage, float attackSpeed) 
    {
        playerMaxHealth = health;
        playerStarterMaxHealth = health;
        playerAttackDamage = attackDamage;
        playerStarterAttackDamage = attackDamage;
        playerAttackSpeed = attackSpeed;
        playerStarterAttackSpeed = attackSpeed;
    }
}

