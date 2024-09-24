using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBarController : MonoBehaviour
{
    public GameObject pan;
    RectTransform rectTransform;

    public float minPos;
    public float maxPos;
    float posDifference;

    private void Start()
    {
        posDifference = maxPos - minPos;
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        float currentForce = pan.GetComponent<PancakeLaunch>().timeSpaceKeyDown;
        float maxForce = pan.GetComponent<PancakeLaunch>().maxForce;
        currentForce = Mathf.Clamp(currentForce, 0, maxForce);

        float newPos = (currentForce / maxForce) * posDifference;

        rectTransform.localPosition = new Vector2(rectTransform.localPosition.x, newPos + minPos);
    }
}
