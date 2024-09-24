using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PancakeManager : MonoBehaviour
{
    public GameObject originalPancake;
    public int score;
    public TextMeshProUGUI scoreCounter;
    void Start()
    {
        spawnNewPancake();
    }

    public void spawnNewPancake()
    {
        GameObject newPancake = Instantiate(originalPancake);
        newPancake.SetActive(true);
    }

    public void scoreAPancake(int panScore)
    {
        score += panScore;
        scoreCounter.text = "Score: " + score.ToString("00000");
    }
}
