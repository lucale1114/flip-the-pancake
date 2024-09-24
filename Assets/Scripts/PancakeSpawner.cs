using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PancakeSpawner : MonoBehaviour
{
    public GameObject originalPancake;

    void Start()
    {
        spawnNewPancake(gameObject);
    }

    public void spawnNewPancake(GameObject spawner)
    {
        print(spawner.name);
        GameObject newPancake = Instantiate(originalPancake);
        newPancake.SetActive(true);
    }
}
