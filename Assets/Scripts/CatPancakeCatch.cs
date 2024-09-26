using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CatPancakeCatch : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float catPawOffsetY;
    [SerializeField] float catPawOffsetX;

    [SerializeField] float startPosXRight;
    [SerializeField] float startPosXLeft;

    public PancakeManager pancakeManager;
    public GameObject leftPaw;
    public GameObject rightPaw;
    public GameObject pancake;
    public GameObject catGameObject;
    public Sprite defaultPaws;
    public Sprite catchedPaws;
    public Animator catBlink;
    public bool playedNom;

    public SpriteRenderer leftPawSprite;
    public SpriteRenderer rightPawSprite;
    public AudioClip nomSound;

    public bool catchedPancake;
    bool rightPawInPlace = false;
    bool leftPawInPlace = false;
    bool resetPaws = false;

    Vector2 pancakeLocalPosition;

    private void Start()
    {
        catBlink.SetBool("CatBlink", true);
    }

    private void Update()
    {
        if (catchedPancake)
        {
            pancakeLocalPosition = transform.InverseTransformPoint(pancake.transform.position);

            Vector2 catPos = catGameObject.transform.localPosition;

            Vector3 leftPawLocalPosition = leftPaw.transform.localPosition;
            Vector3 rightPawLocalPosition = rightPaw.transform.localPosition;

            Vector2 leftPawDistance = pancakeLocalPosition - (Vector2)leftPawLocalPosition;
            Vector2 rightPawDistance = pancakeLocalPosition - (Vector2)rightPawLocalPosition;

            Vector2 offsetY = new Vector2(0, catPawOffsetY);

            if (!leftPawInPlace || !rightPawInPlace)
            {

                if (leftPawDistance.sqrMagnitude != 1f)
                {
                    Vector2 newPosition = Vector2.MoveTowards((Vector2)leftPawLocalPosition, pancakeLocalPosition - offsetY, moveSpeed * Time.deltaTime);
                    leftPaw.transform.localPosition = new Vector3(newPosition.x, newPosition.y, leftPawLocalPosition.z);
                }
                else
                {
                    leftPawInPlace = true;

                }

                if (rightPawDistance.sqrMagnitude != 1f)
                {
                    Vector2 newPosition = Vector2.MoveTowards((Vector2)rightPawLocalPosition, pancakeLocalPosition - offsetY, moveSpeed * Time.deltaTime);
                    rightPaw.transform.localPosition = new Vector3(newPosition.x, newPosition.y, rightPawLocalPosition.z);
                }
                else
                {
                    rightPawInPlace = true;
                }
            }

            Vector2 catDistance = catPos - (Vector2)leftPawLocalPosition;

            if (leftPawInPlace && rightPawInPlace && catDistance.sqrMagnitude != 1f)
            {
                Vector2 newPosition = Vector2.MoveTowards((Vector2)leftPawLocalPosition, catPos - offsetY, moveSpeed * Time.deltaTime);
                leftPaw.transform.localPosition = new Vector3(newPosition.x, newPosition.y, leftPawLocalPosition.z);

                Vector2 newPosition2 = Vector2.MoveTowards((Vector2)rightPawLocalPosition, catPos - offsetY, moveSpeed * Time.deltaTime);
                rightPaw.transform.localPosition = new Vector3(newPosition2.x, newPosition2.y, rightPawLocalPosition.z);

                Vector2 catGlobalPos = catGameObject.transform.position;

                pancake.transform.position = Vector2.MoveTowards(pancake.transform.position, catGlobalPos, moveSpeed * Time.deltaTime * 5);
            }
            else if (leftPawInPlace && rightPawInPlace)
            {
                resetPaws = true;
            }

            if (resetPaws)
            {
                pancake.SetActive(false);
                if (!playedNom)
                {
                    playedNom = true;
                    gameObject.GetComponent<AudioSource>().clip = nomSound;
                    gameObject.GetComponent<AudioSource>().Play();
                }

                leftPawSprite.sprite = defaultPaws;
                rightPawSprite.sprite = defaultPaws;
                leftPawSprite.sortingOrder = 0;
                rightPawSprite.sortingOrder = 0;



                if (leftPawLocalPosition.x > startPosXLeft && rightPawLocalPosition.x < startPosXRight)
                {
                    if (leftPawLocalPosition.x > startPosXLeft)
                    {
                        
                        leftPaw.transform.localPosition += new Vector3(-moveSpeed * Time.deltaTime * 1.2f, 0, 0);
                    }
                    if (rightPawLocalPosition.x < startPosXRight)
                    {
                        rightPaw.transform.localPosition += new Vector3(moveSpeed * Time.deltaTime * 1.2f, 0, 0);
                    }
                }
                else
                {
                    Destroy(pancake);
                    rightPawInPlace = false;
                    leftPawInPlace = false;
                    resetPaws = false;
                    catchedPancake = false;
                    Invoke("spawnNewPancake", 0.5f);
                }
            }
        }
    }

    private void spawnNewPancake()
    {
        playedNom = false;
        pancakeManager.spawnNewPancake();
    }
}
