using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PancakeChecker : MonoBehaviour
{
    public PancakeSpawner spawner;
    public Collider2D trigger;
    public bool canCheck;

    private void OnTriggerEnter2D(Collider2D other)
    {
        print("enter");
        if (other.gameObject.CompareTag("Pancake"))
        {
            print("pancake");
            other.GetComponent<PancakeControls>().validPosition = true;
            other.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            Invoke("newPancake", 2);
        }
    }
    private void newPancake()
    {
        spawner.spawnNewPancake();
    }

    public IEnumerator pancakeCheck()
    {
        yield return new WaitForSeconds(1.5f);
        trigger.enabled = true;
        yield return new WaitForSeconds(0.05f);
        trigger.enabled = false;
    }
}
