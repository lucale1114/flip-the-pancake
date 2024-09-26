using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menu;
    public GameObject credits;

    public void ChangeScene()
    {
        SceneManager.LoadScene("Main Scene");
    }

    public void ChangeMenuToCredits(bool changeMenu)
    {
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
        Application.Quit();
    }
}
