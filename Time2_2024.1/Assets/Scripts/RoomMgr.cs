using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomMgr : MonoBehaviour
{
    public GameObject[] roomPrefabs;
    public GameObject[] doors;
    [SerializeField] PolygonCollider2D mainCameraCollider;
    GameObject[] rooms;
    public GameObject PathFinding;

    // Start is called before the first frame update
    void Start()
    {
        rooms = new GameObject[doors.Length];
        for (int i = 0; i < doors.Length; i++) { 
            int rand = Random.Range(0, roomPrefabs.Length);
            GameObject room = Instantiate(roomPrefabs[rand]);
            room.transform.parent = transform.parent;
            room.transform.Translate(100 * i, 20, 0);

            DoorController doorMain = doors[i].GetComponentInChildren<DoorController>(); // porta na sala principal
            DoorController doorSub = room.GetComponentInChildren<DoorController>(); // porta na sala secundaria
            PolygonCollider2D cameraCollider = room.GetComponent<PolygonCollider2D>();

            foreach (Transform tMain in doors[i].GetComponentsInChildren<Transform>())
            {
                if (tMain.gameObject.name.Equals("TeleportDestination"))
                {
                    foreach (Transform tSub in room.GetComponentsInChildren<Transform>())
                    {
                        if (tSub.gameObject.name.Equals("PlayerTransform"))
                        {
                            doorMain.desiredPlayerLocation = tSub;
                            doorMain.cameraCollider = cameraCollider;

                            doorSub.desiredPlayerLocation = tMain;
                            doorSub.cameraCollider = mainCameraCollider;
                        }
                    }
                }
            }


            

            rooms[i] = room; // Guarda a sala geradas caso precisemos no futuro
        }
        Instantiate(PathFinding);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
