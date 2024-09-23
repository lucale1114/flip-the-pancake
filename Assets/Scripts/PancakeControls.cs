using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Rigidbody2D rb;
    readonly float PANCAKE_FLIP_SPEED = 1.6f;
    void Start()
    {
        rb.AddForce(new Vector2(400, 500));
        Time.timeScale = 0.5f;
    }

    void Update()
    {
        float rotateDirection = Input.GetAxis("Horizontal");
        rb.SetRotation(rb.rotation + -rotateDirection * PANCAKE_FLIP_SPEED);
    }
}
