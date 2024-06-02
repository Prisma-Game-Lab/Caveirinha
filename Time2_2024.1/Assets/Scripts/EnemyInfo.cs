using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class EnemyInfo : ScriptableObject
{
    public string enemyName;
    public string enemyClass;
    public int enemyHealth;
    public int enemyMaxHealth;
    public int enemyAttack;
    public Sprite enemySprite;
}
