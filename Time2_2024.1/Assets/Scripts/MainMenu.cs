using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //Comentario
    public void PlayGame()
    {
        int sfx = (Random.Range(1, 2));
        string name = "PLAY" + sfx.ToString();
        AudioManager.instance.PlaySFX(name);
        SceneManager.LoadScene("Andar1"); //alterar para cena do jogo depois 
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
        SceneManager.LoadScene("TelaInicial");
    }
}

