using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text attackText;
    [SerializeField] private TMP_Text attackSpeedText;
    [SerializeField] private Image itemImage;
    [SerializeField] public Animator CaveiraoAnim;
    [SerializeField] private Sprite[] spriteVector;
 
    public void UpdateUI(float maxHealth, float health, float attackDamage, float attackSpeed) 
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = health;
        healthText.text = $"{Mathf.Round(health)}/{maxHealth}";
        attackText.text = attackDamage.ToString();
        attackSpeedText.text = attackSpeed.ToString();
    }

    public void UpdateItem(int selectedItem, int potionCharges)
    {
        if (selectedItem == 0) 
        {
            itemImage.sprite = spriteVector[potionCharges];
            return;
        }
        itemImage.sprite = spriteVector[selectedItem+2];
    }
}
