using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text attackText;
    [SerializeField] private TMP_Text attackSpeedText;
    [SerializeField] private Image itemImage;
    [SerializeField] private Sprite[] spriteVector;
 
    public void UpdateUI(float maxHealth, float health, float attackDamage, float attackSpeed, int selectedItem) 
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = health;
        healthText.text = $"{health}/{maxHealth}";
        attackText.text = attackDamage.ToString();
        attackSpeedText.text = attackSpeed.ToString();
        itemImage.sprite = spriteVector[selectedItem];
    }
}
