using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoPlayerScript : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer;

    private void OnEnable()
    {
        videoPlayer.loopPointReached += LoadNextScene;
    }

    private void OnDisable()
    {
        videoPlayer.loopPointReached -= LoadNextScene;
    }

    private void LoadNextScene(VideoPlayer vp) 
    {
        SceneManager.LoadScene("Lore…pica");
    }
}
