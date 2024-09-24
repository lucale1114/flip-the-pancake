using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PancakeChecker : MonoBehaviour
{
    public PancakeSpawner spawner;
    public Collider2D trigger;
    public bool canCheck;
    public GameObject pancake = null;
    public bool isInBox = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (pancake == null)
        {
            pancake = other.gameObject;
        }
        print("ENTERED");
        isInBox = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        print("EXITED");
        isInBox = false;
    }
    private void newPancake()
    {
        spawner.spawnNewPancake(gameObject);
    }

    public IEnumerator pancakeCheck()
    {
        print("checkinit");
        yield return new WaitForSeconds(1.5f);
        if (isInBox) 
        {
            pancake.GetComponent<PancakeControls>().validPosition = true;
            print("pancake");
            pancake.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            Invoke("newPancake", 1);
        }
        canCheck = true;
    }
}
