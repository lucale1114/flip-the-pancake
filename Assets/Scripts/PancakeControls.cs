using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Rigidbody2D rb;
    readonly float PANCAKE_FLIP_SPEED = -4f;
    readonly float HORIZONTAL_MOVESPEED = 155f;

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

    IEnumerator checkPancakeValid(Vector3 startPos, PlateScript plate)
    {
        yield return new WaitForSeconds(1);
        RaycastHit2D raycastHit2D = Physics2D.Raycast(startPos, -transform.right, 100);
        Debug.DrawRay(startPos, -transform.right, Color.red, 5);
        if (raycastHit2D.collider == null)
        {
            print("not valid");
            Destroy(gameObject);
        }
        if (raycastHit2D.collider.gameObject.CompareTag("Pancake"))
        {
            print("is valid!");
            if (plate)
            {
                plate.firstPancake = gameObject;
            }
            rb.bodyType = RigidbodyType2D.Static;

        }
        else
        {
            print("not valid");
            Destroy(gameObject);
        }
    }

    IEnumerator checkPancakeValid(Vector3 startPos)
    {
        yield return new WaitForSeconds(1);
        RaycastHit2D raycastHit2D = Physics2D.Raycast(startPos, startPos + new Vector3(0,1));
        if (raycastHit2D.collider.CompareTag("Pancake"))
        {
            print("is valid!");
            rb.bodyType = RigidbodyType2D.Static;

        }
        else
        {
            print("not valid");
            Destroy(gameObject);
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
                print("landed");
                StartCoroutine(checkPancakeValid(other.transform.position, plateScript));
            }
        }
    }
}
