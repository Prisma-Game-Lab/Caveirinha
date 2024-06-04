using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomMgr : MonoBehaviour
{
    public GameObject[] roomPrefabs;
    public GameObject[] doors;
    GameObject[] rooms; 

    // Start is called before the first frame update
    void Start()
    {
        rooms = new GameObject[doors.Length];
        for (int i = 0; i < doors.Length; i++) { 
            int rand = Random.Range(0, roomPrefabs.Length);
            GameObject room = Instantiate(roomPrefabs[rand]);
            room.transform.parent = transform.parent;
            room.transform.Rotate(0, 0, 45 * i);
            room.transform.Translate(0, 20, 0);


            rooms[i] = room; // Guarda a sala geradas caso precisemos no futuro
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
