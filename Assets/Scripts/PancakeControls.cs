using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PancakeControls : MonoBehaviour
{
    public Rigidbody2D rb;
    readonly float PANCAKE_FLIP_SPEED = 10f;
    readonly float HORIZONTAL_MOVESPEED = 155f;
    public bool validPosition = false;
    public bool nextToCheck = false;
    private bool canMove = true;
    private float rotationAcceleration = 0;
    public PancakeSpawner spawner;
    private float deleteTimer;
    private bool deleteStarted = false;
    private bool jumped = false;
    void Start()
    {
        //rb.AddForce(new Vector2(400, 500));
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
        rotationAcceleration = Mathf.Clamp(rotationAcceleration, -1.5f, 1.5f);
        axisVerticalDirection = Mathf.Max(axisVerticalDirection, 0);

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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") || validPosition)
        {
            return;
        }
        if ((other.gameObject.CompareTag("Plate") || other.gameObject.CompareTag("Pancake")) && canMove)
        {
            PancakeChecker plateScript = other.gameObject.GetComponent<PancakeChecker>();
            if (plateScript.canCheck)
            {
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
            deleteTimer += 0.2f;
        }
        if (deleteStarted)
        {
            StartCoroutine(delete());
            deleteStarted = false;
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
            spawner.spawnNewPancake(gameObject);
            Destroy(gameObject);
        }
    }
}
