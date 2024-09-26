using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ApplyScore : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private void Start()
    {
        scoreText.text = "Score: " + GameManager.instance.gameScore.ToString("00000");
    }
}
