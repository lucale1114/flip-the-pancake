using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PancakeControls : MonoBehaviour
{
    public Rigidbody2D rb;
    readonly float PANCAKE_FLIP_SPEED = 13f;
    readonly float HORIZONTAL_MOVESPEED = 155f;
    readonly float MAX_FLIP = 1.5f;
    public bool validPosition = false;
    public bool canMove = false;
    private float rotationAcceleration = 0;
    public PancakeManager gameManager;
    public int pancakeScore = 1000;
    private float deleteTimer;
    private bool deleteStarted = false;
    private bool jumped = false;
    private bool triggeredCheck = false;
    private AudioSource flipSound;
    public AudioClip splatSound;
    private PancakeChecker currentChecker;
    float rotationForFlip = 0;

    void Start()
    {
        flipSound = GetComponent<AudioSource>();
        //rb.AddForce(new Vector2(400, 500));
    }

    private void checkForFlip()
    {
        rotationForFlip += rotationAcceleration;
        if (Mathf.Abs(rotationForFlip) > 360 )
        {
            flipSound.Play();
            pancakeScore = Mathf.RoundToInt(pancakeScore * 1.2f);
            rotationForFlip = 0;
        }
    }

    private void MovementControls()
    {
        float axisHorizontalDirection = Input.GetAxis("Horizontal");
        float axisVerticalDirection = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.UpArrow) && !jumped)
        {
            jumped = true;
            rb.velocity = new Vector3(rb.velocity.x, 10);
        }

        rotationAcceleration += (axisHorizontalDirection * -PANCAKE_FLIP_SPEED) * Time.deltaTime;
        rotationAcceleration = Mathf.Clamp(rotationAcceleration, -MAX_FLIP, MAX_FLIP);
        axisVerticalDirection = Mathf.Min(axisVerticalDirection, 0);
        checkForFlip();
        rb.SetRotation(rb.rotation + rotationAcceleration);
        rb.AddForce(new Vector2(axisHorizontalDirection * HORIZONTAL_MOVESPEED * Time.deltaTime, axisVerticalDirection / 3f));

        if (rotationAcceleration > 0)
        {
            rotationAcceleration -= 0.002f;
        }
        else if (rotationAcceleration < 0)
        {
            rotationAcceleration += 0.002f;
        }
    }

    void Update()
    {
        if (!canMove)
        {
            return;
        }
        MovementControls();
    }

    private void playSplat(float volume)
    {
        GameObject splatObj = new GameObject("splat");
        AudioSource splatAudio = splatObj.AddComponent<AudioSource>();
        splatAudio.clip = splatSound;
        splatAudio.volume = volume;
        splatAudio.pitch = Random.Range(0.75f, 1.5f);
        if (volume > 1)
        {
            splatAudio.pitch = 0.66f;
        }
        splatAudio.Play();
        Destroy(splatObj, 2);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") || validPosition)
        {
            return;
        }

        playSplat(0.25f);
  
        if (other.gameObject.CompareTag("Plate") && !triggeredCheck)
        {
            PancakeChecker plateScript = other.gameObject.GetComponent<PancakeChecker>();
            if (plateScript.canCheck)
            {
                currentChecker = plateScript;
                if (deleteTimer != 0)
                {
                    deleteTimer += 0.5f;
                }
                triggeredCheck = true;
                plateScript.canCheck = false;
                StartCoroutine(plateScript.pancakeCheck());
            }
        }
        
        canMove = false;
        if (deleteTimer == 0)
        {
            deleteStarted = true;
            deleteTimer += 2f;
        } 
        else
        {
            deleteTimer += 0.1f;
        }
        if (deleteStarted)
        {
            StartCoroutine(delete());
            deleteStarted = false;
        }
        if (other.gameObject.name == "KillArea")
        {
            playSplat(5);
            deleteTimer = 0;
        }

    }

    IEnumerator delete()
    {
        while (deleteTimer > 0)
        {
            yield return new WaitForSeconds(0.1f);
            deleteTimer -= 0.1f;
        }
        if (!validPosition)
        {
            gameManager.spawnNewPancake();
            if (currentChecker != null)
            {
                currentChecker.canCheck = true;
            }
            Destroy(gameObject);
        }
    }
}
