using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : DoorController
{
    [SerializeField] int neededSouls = 2;

    private void OnEnable()
    {
        SoulScript.onSoulAssimilation += toggleLock;
    }

    private void OnDisable()
    {
        SoulScript.onSoulAssimilation -= toggleLock;
    }

    public override void toggleLock()
    {
        if (neededSouls <= 0) 
        {
            doorCollider.enabled = true;
            doorSr.color = Color.red;
        }
        else 
        {
            neededSouls -= 1;
        }
    }
}
