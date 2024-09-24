using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PancakeControls : MonoBehaviour
{
    public Rigidbody2D rb;
    readonly float PANCAKE_FLIP_SPEED = -4f;
    readonly float HORIZONTAL_MOVESPEED = 155f;
    public bool validPosition = false;

    private bool canMove = true;
    private float rotationAcceleration = 0;

    void Start()
    {
        rb.AddForce(new Vector2(400, 500));
    }

    private void MovementControls()
    {
        float axisHorizontalDirection = Input.GetAxis("Horizontal");
        float axisVerticalDirection = Input.GetAxis("Vertical");

        rotationAcceleration += (axisHorizontalDirection * PANCAKE_FLIP_SPEED) * Time.deltaTime;
        rotationAcceleration = Mathf.Clamp(rotationAcceleration, -1.5f, 1.5f);
        rb.SetRotation(rb.rotation + rotationAcceleration);
        rb.AddForce(new Vector2(axisHorizontalDirection * HORIZONTAL_MOVESPEED * Time.deltaTime, axisVerticalDirection / 3f));
    }

    void Update()
    {
        if (!canMove)
        {
            return;
        }
        MovementControls();
        if (rotationAcceleration > 0)
        {
            rotationAcceleration -= 0.002f;
        }
        else if (rotationAcceleration < 0)
        {
            rotationAcceleration += 0.002f;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Plate") && canMove)
        {
            PlateScript plateScript = other.gameObject.GetComponent<PlateScript>();
            if (plateScript.firstPancake == null)
            {
                canMove = false;
                Invoke("delete", 1.5f);
                StartCoroutine(plateScript.pancakeCheck());
            }
        }
    }

    private void delete()
    {
        if (!validPosition)
        {
            Destroy(gameObject);
        }
    }
}
