using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Rigidbody2D rb;
    readonly float PANCAKE_FLIP_SPEED = -5f;
    readonly float HORIZONTAL_MOVESPEED = 105f;
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
        Mathf.Clamp(rotationAcceleration, -1, 1);
        rb.SetRotation(rb.rotation + rotationAcceleration);
        rb.AddForce(new Vector2(axisHorizontalDirection * HORIZONTAL_MOVESPEED * Time.deltaTime, axisVerticalDirection /3f));
    }

    void Update()
    {
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
}
