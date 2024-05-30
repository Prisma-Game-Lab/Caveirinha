using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(0); //alterar para cena do jogo depois 
    }

    public void QuitGame()
    {
        Debug.Log("o jogo fechou"); //teste - tirar depois
        Application.Quit();
    }

}

