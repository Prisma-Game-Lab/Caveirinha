using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : DoorController
{
    public int roomsNeeded;

    private void OnEnable()
    {
        if(GameManager.instance == null) 
        {
            return;
        }
        if(GameManager.instance.RoomClearedThisFloor >= roomsNeeded) 
        {
            toggleLock();
        }
    }

    public override void toggleLock()
    {
        doorCollider.enabled = true;
        doorSr.color = Color.red;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(BossTransition(collision.transform));
        }
    }

    private IEnumerator BossTransition(Transform playerTransform)
    {
        DesiredRoom.SetActive(true);
        CanvasController.onSceneTransition();
        PlayerController playerScript = playerTransform.gameObject.GetComponent<PlayerController>();
        playerScript.OnDoorEnter();
        yield return new WaitForSeconds(0.85f);
        cinemachineTransposer.m_XDamping = 0;
        cinemachineTransposer.m_YDamping = 0;
        cinemachineTransposer.m_ZDamping = 0;
        playerTransform.position = desiredPlayerLocation.position;
        cameraConfiner.m_BoundingShape2D = cameraCollider;
        cameraObject.transform.position = desiredPlayerLocation.position;
        CurrentRoom.SetActive(false);
        toggleLock();
        yield return new WaitForSeconds(0.1f);
        cinemachineTransposer.m_XDamping = 1;
        cinemachineTransposer.m_YDamping = 1;
        cinemachineTransposer.m_ZDamping = 1;
    }
}
