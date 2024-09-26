using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menu;
    public GameObject credits;
    public GameObject fadeScreen;
    public AudioSource hoverSound;
    public AudioSource pressedSound;
    public GameObject musicObject;
    public GameObject sadCat;

    public void ChangeScene()
    {
        if (SceneManager.GetActiveScene().name != "End Scene")
        {
            DontDestroyOnLoad(musicObject);
        }
        pressedSound.Play();
        fadeScreen.SetActive(true);
        fadeScreen.GetComponent<Animator>().SetBool("Fade", true);
        Invoke("newScene", 1);
    }

    void newScene()
    {
        SceneManager.LoadScene("Main Scene");
    }

    public void MouseEntered()
    {
          hoverSound.Play();
    }

    public void ChangeMenuToCredits(bool changeMenu)
    {
        pressedSound.Play();
        if (changeMenu)
        {
            menu.SetActive(false);
            credits.SetActive(true);
        }
        else
        {
            menu.SetActive(true);
            credits.SetActive(false);
        }
    }

    public void QuitGame()
    {
        pressedSound.Play();
        Application.Quit();
    }

    public void ActivateHappyCatScreen()
    {
        sadCat.SetActive(true);
    }
}
