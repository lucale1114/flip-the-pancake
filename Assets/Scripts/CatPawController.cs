using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatPawController : MonoBehaviour
{
    [SerializeField] public int speed;
    [SerializeField] float minPos;
    [SerializeField] float maxPos;

    Vector2 nextPos;

    private void Start()
    {
        GenerateNewPos();
    }

    void Update()
    {
        if (nextPos.y > transform.position.y)
        {
            transform.position += new Vector3(0, speed * Time.deltaTime, 0);
        }
        if (nextPos.y < transform.position.y)
        {
            transform.position += new Vector3(0, -speed * Time.deltaTime, 0);
        }

        Vector2 distance = (Vector2)transform.position - nextPos; 

        if (Vector2.SqrMagnitude(distance) < 0.1f)
        {
            GenerateNewPos();
        }
    }
     
    void GenerateNewPos()
    {
        nextPos = new Vector2(transform.position.x, Random.Range(minPos, maxPos));
    }
}
