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
            room.transform.Translate(20 * i, 20, 0);

            DoorController doorMain = doors[i].GetComponentInChildren<DoorController>(); // porta na sala principal
            DoorController doorSub = room.GetComponentInChildren<DoorController>(); // porta na sala secundaria

            foreach (Transform tMain in doors[i].GetComponentsInChildren<Transform>())
            {
                if (tMain.gameObject.name.Equals("PlayerTransform (2)"))
                {
                    foreach (Transform tSub in room.GetComponentsInChildren<Transform>())
                    {
                        if (tSub.gameObject.name.Equals("PlayerTransform"))
                        {
                            doorMain.desiredPlayerLocation = tSub;
                            doorMain.desiredCameraLocation = room.GetComponent<Transform>();

                            doorSub.desiredPlayerLocation = tMain;
                            doorSub.desiredCameraLocation = transform;
                        }
                    }
                }
            }


            

            rooms[i] = room; // Guarda a sala geradas caso precisemos no futuro
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
