using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PancakeLaunch : MonoBehaviour
{
    [SerializeField] int launchForce;

    float pancakeOffsetY;
    [SerializeField] float timeSpaceKeyDown;
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
        if (Input.GetKey(KeyCode.Space))
        {
            timeSpaceKeyDown += Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            rb.velocity = new Vector2(0, launchForce * timeSpaceKeyDown);
            pancakeOffsetY = timeSpaceKeyDown;
            newPos = originalPos + new Vector2(0, pancakeOffsetY);
            timeSpaceKeyDown = 0;
        }

        if (transform.position.y > newPos.y)
        {
            rb.MovePosition(originalPos);
            rb.velocity = Vector2.zero;
        }
    }
}
