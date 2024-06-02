using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUpdate : MonoBehaviour
{
    public EnemyInfo enemy;
    public Text nameText;
    public Text classText;
    public Text healthText;
    public Text mHealthText;
    public Image artwork;

    void Start()
    {
        nameText.text = enemy.enemyName;
        classText.text = enemy.enemyClass;
        healthText.text = enemy.enemyHealth.ToString();
        mHealthText.text = enemy.enemyMaxHealth.ToString();
        artwork.sprite = enemy.enemySprite;
    }
}
