using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public delegate void OnRoomCleared();
    public static OnRoomCleared onRoomCleared;
    [SerializeField] DoorController[] doors;
    public string enemyType;
    bool roomNotCleared;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
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
        if (enemyType == "Boss")
        {
            return;
        }
        foreach(DoorController door in doors) 
        {
            door.toggleLock();
        }
    }

    private void FixedUpdate()
    {
        if (gameObject.transform.childCount == 0 && !roomNotCleared) 
        {
            onRoomCleared();
            roomNotCleared = true;

        }
        
    }
}
