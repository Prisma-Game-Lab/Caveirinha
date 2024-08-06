using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DificultySelector : MonoBehaviour
{
    [SerializeField] private float[] starterHealth = {200,150,100};
    [SerializeField] private float[] starterAttackDamage = { 75, 55, 35 };
    [SerializeField] private float[] starterAttackSpeed = { 2, 1.5f, 1 };

    [SerializeField] private Button[] buttons;

    public void DificultyButton(int dificultyIndex) 
    {
        foreach(Button button in buttons) 
        {
            button.enabled = false;
        }
        GameManager.instance.SetDificulty(starterHealth[dificultyIndex], starterAttackDamage[dificultyIndex], starterAttackSpeed[dificultyIndex]);
    }
}
