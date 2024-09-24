using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateScript : MonoBehaviour
{
    public GameObject firstPancake = null;
    public Collider2D trigger;
    private void OnTriggerEnter2D(Collider2D other)
    {
        print("enter");
        if (other.gameObject.CompareTag("Pancake"))
        {
            print("pancake");
            other.GetComponent<PancakeControls>().validPosition = true;
            firstPancake = other.gameObject;
            other.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
    }

    public IEnumerator pancakeCheck()
    {
        yield return new WaitForSeconds(1f);
        trigger.enabled = true;
        yield return new WaitForSeconds(0.05f);
        trigger.enabled = false;
    }
}
