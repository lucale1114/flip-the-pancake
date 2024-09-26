using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ApplyScore : MonoBehaviour
{
    private void Start()
    {
        GetComponent<TextMeshPro>().text = "Score: " + GameManager.instance.gameScore.ToString("00000");
    }
}
