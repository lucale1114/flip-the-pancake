using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatPawController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float minPos;
    [SerializeField] float maxPos;

    public GameObject Cat;
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
        if(Cat.GetComponent<CatPancakeCatch>().catchedPancake == false)
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
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pancake"))
        {
            other.GetComponent<Rigidbody2D>().gravityScale = 0;
            other.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            other.GetComponent<PancakeControls>().canMove = false;
            Cat.GetComponent<CatPancakeCatch>().catchedPancake = true;
            Cat.GetComponent<CatPancakeCatch>().pancake = other.gameObject;

        }
    }

    void GenerateNewPos()
    {
        nextPos = new Vector2(transform.localPosition.x, Random.Range(minPos, maxPos));
    }
}
