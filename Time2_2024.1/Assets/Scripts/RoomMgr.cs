using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomMgr : MonoBehaviour
{
    public GameObject[] roomPrefabs;
    public GameObject[] doors;

    [SerializeField] private GameObject mainRoom;
    public GameObject PathFinding;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] rooms = new GameObject[doors.Length];
        PolygonCollider2D mainRoomCollider = mainRoom.GetComponent<PolygonCollider2D>();
        for (int i = 0; i < doors.Length; i++)
        {
            int rand = Random.Range(0, roomPrefabs.Length);
            GameObject room = Instantiate(roomPrefabs[rand]);
            //room.transform.parent = transform.parent;
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
                            doorMain.CurrentRoom = mainRoom;
                            doorMain.DesiredRoom = room;
                            doorMain.cameraCollider = cameraCollider;

                            doorSub.desiredPlayerLocation = tMain;
                            doorSub.CurrentRoom = room;
                            doorSub.DesiredRoom = mainRoom;
                            doorSub.cameraCollider = mainRoomCollider;
                        }
                    }
                }
            }
            rooms[i] = room;
        }
        Instantiate(PathFinding);
        foreach(GameObject room in rooms) 
        {
            room.SetActive(false);
        }
    }
}
