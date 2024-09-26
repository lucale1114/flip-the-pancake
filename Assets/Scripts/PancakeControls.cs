using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PancakeControls : MonoBehaviour
{
    public Rigidbody2D rb;
    readonly float PANCAKE_FLIP_SPEED = 1770f;
    readonly float HORIZONTAL_MOVESPEED = 300f;
    readonly float DEACCELERATION = 10f;
    readonly float MAX_FLIP = 13f;
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
    private AudioSource jumpSound;
    private AudioSource errorSound;
    public AudioClip splatSound;
    private PancakeChecker currentChecker;
    float rotationForFlip = 0;
    private bool fallen = false;

    void Start()
    {
        flipSound = GetComponents<AudioSource>()[0];
        jumpSound = GetComponents<AudioSource>()[1];
        errorSound = GetComponents<AudioSource>()[2];
        //rb.AddForce(new Vector2(400, 500));
    }

    private void checkForFlip()
    {
        rotationForFlip += rotationAcceleration;
        if (Mathf.Abs(rotationForFlip) > 4200)
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

        if ((axisVerticalDirection > 0 || Input.GetKeyDown(KeyCode.Space)) && !jumped)
        {
            jumped = true;
            jumpSound.Play();
            rb.velocity = new Vector3(rb.velocity.x, 10);
        }

        rotationAcceleration += (axisHorizontalDirection * -PANCAKE_FLIP_SPEED) * Time.deltaTime;
        rotationAcceleration = Mathf.Clamp(rotationAcceleration, -MAX_FLIP, MAX_FLIP);
        axisVerticalDirection = Mathf.Min(axisVerticalDirection, 0);
        checkForFlip();
        rb.SetRotation(rb.rotation + (rotationAcceleration * Time.deltaTime) * 50);
        rb.AddForce(new Vector2(axisHorizontalDirection * HORIZONTAL_MOVESPEED * Time.deltaTime, axisVerticalDirection / 3f));

        if (rotationAcceleration > 0)
        {
            rotationAcceleration -= DEACCELERATION * Time.deltaTime;
        }
        else if (rotationAcceleration < 0)
        {
            rotationAcceleration += DEACCELERATION * Time.deltaTime;
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
        if ((other.gameObject.CompareTag("Player") && canMove == false) || validPosition)
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
            fallen = true;
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
            if (!fallen)
            {
                GameObject splatObj = new GameObject("Error");
                AudioSource audio = splatObj.AddComponent<AudioSource>();
                audio.clip = errorSound.clip;
                audio.Play();
            }

            gameManager.spawnNewPancake();
            if (currentChecker != null)
            {
                currentChecker.canCheck = true;
            }
            Destroy(gameObject);


        }
    }
}
