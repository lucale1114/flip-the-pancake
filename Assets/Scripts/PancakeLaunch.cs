using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PancakeLaunch : MonoBehaviour
{
    [SerializeField] int launchForce;
    [SerializeField] int moveToOriginalPosSpeed;
    [SerializeField] int panRotationSpeed;
    public int maxForce;
    public float timeSpaceKeyDown;

    float pancakeOffset;
    bool selectForce = true;
    bool moveToOriginalPos;
    bool rotatePan;
    public bool pancakeSpawned;
    public Animator tutorialFader;
    private bool playedAnimation;

    Vector2 originalPos;

    public PancakeControls currentPancake;
    public GameObject pivot;
    public Rigidbody2D rb;
    

    private void Start()
    {
        originalPos = pivot.transform.position;
    }

    private void Update()
    {
        PlayerInput();
        RotatePan();
        CheckMaxDistance();
        MoveToOriginalPos();
    }

    void PlayerInput()
    {
        if (pancakeSpawned)
        {
            if (selectForce)
            {
                if (Input.GetKey(KeyCode.Space) && !moveToOriginalPos && !rotatePan)
                {
                    timeSpaceKeyDown += Time.deltaTime;
                    timeSpaceKeyDown = Mathf.Clamp(timeSpaceKeyDown, 0, maxForce);
                }

                if (Input.GetKeyUp(KeyCode.Space) && !rotatePan)
                {
                    selectForce = false;
                    rotatePan = true;
                }
            }
        }
    }

    void RotatePan()
    {
        float axis = Input.GetAxisRaw("Vertical");
        if (rotatePan)
        {
            if (axis > 0)
            {
                pivot.transform.Rotate(Vector3.forward * panRotationSpeed * Time.deltaTime);
            }
            else if (axis < 0)
            {
                pivot.transform.Rotate(-Vector3.forward * panRotationSpeed * Time.deltaTime);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!playedAnimation)
                {
                    playedAnimation = true;
                    tutorialFader.SetBool("Fade", true);
                    print("played");
                }
                rotatePan = false;
                LaunchPancake();
            }
        }
    }

    IEnumerator CanMovePancake(PancakeControls currentPancake)
    {
        yield return new WaitForSeconds(0.2f);
        currentPancake.canMove = true;
    }

    void LaunchPancake()
    {
        pancakeSpawned = false;

        rb.velocity = (Vector2)pivot.transform.up * launchForce * timeSpaceKeyDown;

        pancakeOffset = timeSpaceKeyDown * 2;

        StartCoroutine(CanMovePancake(currentPancake));

        gameObject.GetComponent<AudioSource>().Play();
        timeSpaceKeyDown = 0;
    }

    void CheckMaxDistance()
    {
        Vector2 distance = (Vector2)pivot.transform.position - originalPos;
        if (Vector2.SqrMagnitude(distance) > pancakeOffset)
        {
            moveToOriginalPos = true;
            rb.velocity = Vector2.zero;
        }
    }

    void MoveToOriginalPos()
    {
        if (moveToOriginalPos)
        {
            Vector2 distance = (Vector2)pivot.transform.position - originalPos;
            if (Vector2.SqrMagnitude(distance) > 0.001f || Mathf.Abs(pivot.transform.rotation.z) > 0.001f)
            {
                if (Vector2.SqrMagnitude(distance) > 0.001f)
                {
                    rb.position = Vector2.MoveTowards(pivot.transform.position, originalPos, moveToOriginalPosSpeed * Time.deltaTime);
                }

                if (Mathf.Abs(pivot.transform.rotation.z) > 0.001f)
                {
                    if (pivot.transform.rotation.z < 0)
                    {
                        pivot.transform.Rotate(Vector3.forward * 40 * Time.deltaTime);
                    }
                    else
                    {
                        pivot.transform.Rotate(-Vector3.forward * 40 * Time.deltaTime);
                    }
                }
            }
            else
            {
                pivot.transform.position = originalPos;
                moveToOriginalPos = false;
                selectForce = true;
                pivot.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }
}
