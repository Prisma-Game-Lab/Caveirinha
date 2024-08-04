using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    [SerializeField] GameObject transitionObject;
    public GameObject circleObject;
    GameObject player;
    PlayerController playerController;
    Animator anim;
    bool isOnTransition;
    bool gameIsPaused;

    [SerializeField] GameObject gameUI;
    [SerializeField] GameObject pauseUI;
    [SerializeField] GameObject gameOverUI;

    public delegate void OnSceneTransition();
    public static OnSceneTransition onSceneTransition;

    public AudioSource music, sfx;
    public Slider musicSl, SFXSl;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        transitionObject.SetActive(true);
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        onSceneTransition += AnimationTransition;
    }

    private void OnDisable()
    {
        onSceneTransition -= AnimationTransition;
    }

    private void FixedUpdate()
    {
        if (isOnTransition) 
        {
            circleObject.transform.position = Camera.main.WorldToScreenPoint(player.transform.position);
        }
    }

    private IEnumerator WaitForAnimation() 
    {
        transitionObject.SetActive(true);
        isOnTransition = true;
        yield return new WaitForSeconds(2);
        isOnTransition = false;
        transitionObject.SetActive(false);
    }

    public void AnimationTransition() 
    {
        anim.SetTrigger("LeavingScene");
        StartCoroutine(WaitForAnimation());
    }

    public void ResumeButton() 
    {
        playerController.TogglePause();
    }

    public void PauseGame() 
    {
        if (!gameIsPaused) 
        {
            Time.timeScale = 0;
            pauseUI.SetActive(true);
        }
        else 
        {
            Time.timeScale = 1;
            pauseUI.SetActive(false);
        }
        gameIsPaused = !gameIsPaused;
    }

    public void LoadMainMenu() 
    {
        Time.timeScale = 1;
        AudioManager.instance.PlayMusic("MENU");
        SceneManager.LoadScene("TelaInicial");
    }

    public void LoadCurrentScene()
    {
        Time.timeScale = 1;
        AudioManager.instance.PlayMusic("GAME");
        GameManager.instance.ErasePlayerData();
        SceneManager.LoadScene(1);
    }

    public IEnumerator GameOver() 
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(1);
        AudioManager.instance.PlayMusic("DIE");
        gameOverUI.SetActive(true);
    }

    public void MusicVolume(float MusicValue)
    {
        AudioManager.instance.MusicVolume(MusicValue);
        music.volume = MusicValue;
    }

    public void SFXVolume(float SFXValue)
    {
        AudioManager.instance.SFXVolume(SFXValue);
        sfx.volume = SFXValue;
    }

    private void Start()
    {
        musicSl.value = AudioManager.instance.musicSource.volume;
        SFXSl.value = AudioManager.instance.sfxSource.volume;
    }
}
