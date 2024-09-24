using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PancakeLaunch : MonoBehaviour
{
    [SerializeField] int launchForce;
    [SerializeField] int moveToOriginalPosSpeed;
    [SerializeField] int panRotationSpeed;
    [SerializeField] float timeSpaceKeyDown;

    float pancakeOffsetY;
    bool selectForce = true;
    bool moveToOriginalPos;
    [SerializeField] bool rotatePan;

    Vector2 originalPos;
    Vector2 newPos;

    public GameObject pivot;
    public Rigidbody2D rb;

    private void Start()
    {
        originalPos = transform.position;
    }

    private void Update()
    {
        PlayerInput();
        RotatePan();
        CheckMaxYDistance();
        MoveToOriginalPos();
    }

    void PlayerInput()
    {
        if (selectForce)
        {
            if (Input.GetKey(KeyCode.Space) && !moveToOriginalPos && !rotatePan)
            {
                timeSpaceKeyDown += Time.deltaTime;
            }

            if (Input.GetKeyUp(KeyCode.Space) && !rotatePan)
            {
                selectForce = false;
                rotatePan = true;
            }
        }
    }

    void RotatePan()
    {
        if (rotatePan)
        {
            if (Input.GetKey("w"))
            {
                pivot.transform.Rotate(Vector3.forward * panRotationSpeed * Time.deltaTime);
            }
            if (Input.GetKey("s"))
            {
                pivot.transform.Rotate(-Vector3.forward * panRotationSpeed * Time.deltaTime);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                rotatePan = false;
                LaunchPancake();
            }
        }
    }

    void LaunchPancake()
    {
        rb.velocity = (Vector2)transform.up * launchForce * timeSpaceKeyDown;

        pancakeOffsetY = timeSpaceKeyDown * 2;
        newPos = originalPos + new Vector2(0, pancakeOffsetY);

        timeSpaceKeyDown = 0;
    }

    void CheckMaxYDistance()
    {
        if (transform.position.y > newPos.y)
        {
            moveToOriginalPos = true;
            rb.velocity = Vector2.zero;
        }
    }

    void MoveToOriginalPos()
    {
        if (moveToOriginalPos)
        {
            Vector2 distance = (Vector2)transform.position - originalPos;
            if (Vector2.SqrMagnitude(distance) > 0.001f || Mathf.Abs(transform.rotation.z) > 0.001f)
            {
                if (Vector2.SqrMagnitude(distance) > 0.001f)
                {
                    rb.position = Vector2.MoveTowards(transform.position, originalPos, moveToOriginalPosSpeed * Time.deltaTime);
                }

                if (Mathf.Abs(transform.rotation.z) > 0.001f)
                {
                    if (transform.rotation.z < 0)
                    {
                        transform.Rotate(Vector3.forward * 40 * Time.deltaTime);
                    }
                    else
                    {
                        transform.Rotate(-Vector3.forward * 40 * Time.deltaTime);
                    }
                }
            }
            else
            {
                rb.position = originalPos;
                moveToOriginalPos = false;
                selectForce = true;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }
}
