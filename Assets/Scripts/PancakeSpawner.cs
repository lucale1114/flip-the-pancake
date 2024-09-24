using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PancakeSpawner : MonoBehaviour
{
    public GameObject originalPancake;

    void Start()
    {
        spawnNewPancake();
    }

    public void spawnNewPancake()
    {
        GameObject newPancake = Instantiate(originalPancake);
        newPancake.SetActive(true);
    }
}
