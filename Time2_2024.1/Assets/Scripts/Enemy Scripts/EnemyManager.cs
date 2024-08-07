using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public delegate void OnRoomCleared();
    public static OnRoomCleared onRoomCleared;
    [SerializeField] DoorController[] doors;

    int enemysAlive;

    private void Awake()
    {
        enemysAlive = gameObject.transform.childCount;
    }

    private void OnEnable()
    {
        SoulScript.onSoulAssimilation += LockDoors;
    }

    private void OnDisable()
    {
        SoulScript.onSoulAssimilation -= LockDoors;
    }

    public void LockDoors() 
    {
        foreach(DoorController door in doors) 
        {
            door.toggleLock();
        }
    }

    public void OnEnemyDeath() 
    {
        if (enemysAlive == 0)
        {
            onRoomCleared();
            return;
        }
        enemysAlive -= 1;
    }
}
