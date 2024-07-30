using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalScript : MonoBehaviour
{
    PlayerController playerController;
    public string sceneName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(SceneTransition(collision.gameObject));
        }
    }

    private IEnumerator SceneTransition(GameObject playerObject) 
    {
        CanvasController.onSceneTransition();
        playerObject.GetComponent<PlayerController>().OnDoorEnter();
        GameManager.instance.SavePlayerStats(playerObject);
        yield return new WaitForSeconds(0.85f);
        SceneManager.LoadScene(sceneName);
    }
}
