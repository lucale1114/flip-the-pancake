using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatPawController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float minPos;
    [SerializeField] float maxPos;

    Vector2 nextPos;

    private void Start()
    {
        GenerateNewPos();
    }

    void Update()
    {
        UpdatePawPos();
    }

    void UpdatePawPos()
    {
        if (nextPos.y > transform.localPosition.y)
        {
            transform.localPosition += new Vector3(0, speed * Time.deltaTime, 0);
        }
        if (nextPos.y < transform.localPosition.y)
        {
            transform.localPosition += new Vector3(0, -speed * Time.deltaTime, 0);
        }

        Vector2 distance = (Vector2)transform.localPosition - nextPos;

        if (Vector2.SqrMagnitude(distance) < 0.01f)
        {
            GenerateNewPos();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Test");
        if (other.CompareTag("Pancake"))
        {

        }
    }

    void GenerateNewPos()
    {
        nextPos = new Vector2(transform.localPosition.x, Random.Range(minPos, maxPos));
    }
}
