using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PancakeSpawner : MonoBehaviour
{
    public GameObject originalPancake;
    public Vector3 spawnPosition;

    void Start()
    {
        spawnPosition = originalPancake.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
