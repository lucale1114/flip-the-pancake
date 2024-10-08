using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PancakeChecker : MonoBehaviour
{
    public PancakeManager gameManager;
    public Collider2D trigger;
    public bool canCheck;
    public GameObject pancake = null;
    public bool isInBox = false;

    public AudioSource audioSource;
    public AudioClip success;
    public AudioClip error;

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
        pancake.GetComponent<PancakeControls>().enabled = false;
        pancake.tag = "Plate";
        gameManager.spawnNewPancake();
    }

    public IEnumerator pancakeCheck()
    {
        yield return new WaitForSeconds(1.5f);
        if (pancake.IsDestroyed() || pancake == null )
        {
            pancake = null;
            yield return false;
        }
        float pancakeRotation = Mathf.Round(Mathf.Abs(pancake.transform.localRotation.eulerAngles.z));
        if (isInBox && (pancakeRotation == 0 || pancakeRotation == 180 || pancakeRotation == 360))
        {
            audioSource.clip = success;
            audioSource.Play();
            pancake.name = "PancakeTable";
            canCheck = false;
            pancake.GetComponent<PancakeControls>().validPosition = true;
            pancake.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            gameManager.scoreAPancake(pancake.GetComponent<PancakeControls>().pancakeScore);
            Invoke("newPancake", 0.5f);
            pancake.GetComponent<PancakeChecker>().canCheck = true;
            this.enabled = false;
        } else
        {
            canCheck = true;
        }
    }
}
