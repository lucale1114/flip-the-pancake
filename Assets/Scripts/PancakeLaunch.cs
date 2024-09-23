using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PancakeLaunch : MonoBehaviour
{
    [SerializeField] int launchForce;
    [SerializeField] float timeSpaceKeyDown;

    float pancakeOffsetY;
    bool moveToOriginalPos;

    Vector2 originalPos;
    Vector2 newPos;

    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalPos = transform.position;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space) && !moveToOriginalPos)
        {
            timeSpaceKeyDown += Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            LaunchPancake();
        }

        if (transform.position.y > newPos.y)
        {
            moveToOriginalPos = true;
            rb.velocity = Vector2.zero;
        }

        MoveToOriginalPos();
    }

    void LaunchPancake()
    {
        rb.velocity = new Vector2(0, launchForce * timeSpaceKeyDown);
        pancakeOffsetY = timeSpaceKeyDown * 2;
        newPos = originalPos + new Vector2(0, pancakeOffsetY);
        timeSpaceKeyDown = 0;
    }

    void MoveToOriginalPos()
    {
        if (moveToOriginalPos)
        {
            Vector2 distance = (Vector2)transform.position - originalPos;
            if (Vector2.SqrMagnitude(distance) > 0.01f)
            {
                rb.position = Vector2.MoveTowards(transform.position, originalPos, 20f * Time.deltaTime);
            }
            else
            {
                rb.position = originalPos;
                moveToOriginalPos = false;
            }
        }
    }
}
