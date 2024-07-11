using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : DoorController
{
    public int roomsNeeded;

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
        if (locked && GameManager.instance.RoomCleared >= roomsNeeded) 
        {
            doorCollider.enabled = true;
            doorSr.color = Color.red;
            locked = false;
        }
    }

    private IEnumerator RoomTransition(Transform playerTransform)
    {
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
        EnemyManager enemies = GameObject.Find("BossManager").GetComponent<EnemyManager>();
        if (enemies != null)
        {
            enemies.gameObject.SetActive(true);
        }
        toggleLock();
        yield return new WaitForSeconds(0.1f);
        cinemachineTransposer.m_XDamping = 1;
        cinemachineTransposer.m_YDamping = 1;
        cinemachineTransposer.m_ZDamping = 1;
    }
}
