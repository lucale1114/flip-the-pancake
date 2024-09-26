using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PancakeManager : MonoBehaviour
{
    public GameObject originalPancake;
    public PancakeLaunch pan;
    public int score;
    public TextMeshProUGUI scoreCounter;
    public int pancakeAmmo = 10;

    void Start()
    {
        spawnNewPancake();
    }

    public void spawnNewPancake()
    {
        if (pancakeAmmo == 0)
        {
            SceneManager.LoadScene("End Scene");
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
    }

    public void scoreAPancake(int panScore)
    {
        score += panScore;
        scoreCounter.text = "Score: " + score.ToString("00000");
    }
}
