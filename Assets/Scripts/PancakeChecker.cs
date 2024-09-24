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
        if (pancake == null && other.gameObject.CompareTag("Pancake"))
        {
            pancake = other.gameObject;
        }
        isInBox = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isInBox = false;
    }
    private void newPancake()
    {
        print("spawn check");
        pancake.GetComponent<PancakeControls>().enabled = false;
        pancake.tag = "Plate";
        spawner.spawnNewPancake();
    }

    public IEnumerator pancakeCheck()
    {
        print("checkinit");
        yield return new WaitForSeconds(1.5f);
        if (isInBox) 
        {
            canCheck = false;
            pancake.GetComponent<PancakeControls>().validPosition = true;
            pancake.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            print("pancake");
            Invoke("newPancake", 1);
            pancake.GetComponent<PancakeChecker>().canCheck = true;
            this.enabled = false;
        } else
        {
            canCheck = true;
        }
    }
}
