using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PancakeManager : MonoBehaviour
{
    public GameObject originalPancake;
    public GameObject fadeScreen;
    public GameObject pancakeLauncher;
    public PancakeLaunch pan;
    public int score;
    public TextMeshProUGUI scoreCounter;

    public int pancakeAmmo = 10;
    public Image pancakeCounter;
    public Sprite[] pancakeSprites;

    void Start()
    {
        spawnNewPancake();
    }

    public void spawnNewPancake()
    {
        if (pancakeAmmo == 0)
        {
            fadeScreen.SetActive(true);
            fadeScreen.GetComponent<Animator>().SetBool("Fade", true);
            Invoke("ChangeScene", 1);
            return;
        }
        if (GameObject.Find("PancakeClone"))
        {
            return;
        }
        GameObject newPancake = Instantiate(originalPancake);
        pan.currentPancake = newPancake.GetComponent<PancakeControls>();
        newPancake.SetActive(true);
        pancakeAmmo--;
        pancakeLauncher.GetComponent<PancakeLaunch>().pancakeSpawned = true;
        pancakeCounter.sprite = pancakeSprites[pancakeAmmo];
        
    }

    public void scoreAPancake(int panScore)
    {
        score += panScore;
        scoreCounter.text = "Score: " + score.ToString("00000");
    }

    public void ChangeScene()
    {
        GameManager.instance.SetScore(score);
        SceneManager.LoadScene("End Scene");
    }
}
