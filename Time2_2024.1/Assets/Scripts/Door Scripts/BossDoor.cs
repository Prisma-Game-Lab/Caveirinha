using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : DoorController
{

    private void OnEnable()
    {
        locked = true;
        SoulScript.onSoulAssimilation += toggleLock;
    }

    private void OnDisable()
    {
        SoulScript.onSoulAssimilation -= toggleLock;
    }

    public override void toggleLock()
    {
        if (locked && GameManager.instance.RoomCleared >= 3) 
        {
            doorCollider.enabled = true;
            doorSr.color = Color.red;
            locked = false;
        }
    }
}
