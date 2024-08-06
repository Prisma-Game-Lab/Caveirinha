using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //Comentario
    private void Awake()
    {
        AudioManager.instance.PlayMusic("MENU");
    }

    public void PlayGame()
    {
        int sfx = Random.Range(1, 2);
        string name = "PLAY" + sfx.ToString();
        AudioManager.instance.PlaySFX(name);
        transform.parent.GetComponent<Animator>().Play("MainMenuTransition");
        StartCoroutine(WaitForTransition());
    }

    private IEnumerator WaitForTransition() 
    {
        yield return new WaitForSeconds(0.9f);
        AudioManager.instance.PlayMusic("GAME");
        SceneManager.LoadScene("Andar1");
    }

    public void QuitGame()
    {
        Debug.Log("o jogo fechou"); //teste - tirar depois
        Application.Quit();
    }

    public void Click()
    {
        AudioManager.instance.PlaySFX("SELECT");
    }

    public void Menu()
    {
        AudioManager.instance.PlayMusic("MENU");
        SceneManager.LoadScene("TelaInicial");
    }

    public void EndSlide()
    {
        SceneManager.LoadScene("TelaInicial");
    }
}

