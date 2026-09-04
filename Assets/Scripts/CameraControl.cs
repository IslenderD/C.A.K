using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
public class CameraControl : MonoBehaviour
{

    public float duration = 1f;
    public AnimationCurve curve;

    [SerializeField] Transform playerTrg;

    public float smoothSpeed = 0.1f;
    public Vector3 offSet;

    Vector3 Velocity = Vector3.zero;

    [SerializeField] float minPosX, maxPosX;
    [SerializeField] float minPosY, maxPosY;

    public bool isReversed = false;
    public bool isUpsideDown = false;
    public bool isSideWays = false;

    public bool isTransitioning;

    public float reversedOffset = 40f;

    public bool omgIsLikeFez = false;
    public Vector3 sideViewOffset = new Vector3(20f, 0f, 0f);

    [Header("Random")]
    public bool randomizer = true;
    public float randomInterval = 5f;
    public float normalPeriod = 5f; 
    private float randomTimer = 0f;
    private float timeSinceStart = 0f;
    private bool normalPeriodOver = false;

    [Header("UI")]
    public TextMeshProUGUI timerText;


    // Update is called once per frame
    void Update()
    {
        // --- 1. Timer & Randomizer Logic ---
        if (randomizer)
        {
            if (!normalPeriodOver)
            {
                // Grace Period (First Y seconds)
                timeSinceStart += Time.deltaTime;

                if (timerText != null)
                {
                    float timeRemaining = Mathf.Max(0, normalPeriod - timeSinceStart);
                    timerText.text = timeRemaining.ToString("F1");
                    timerText.color = Color.white;
                }

                if (timeSinceStart >= normalPeriod)
                {
                    normalPeriodOver = true;
                    TriggerRandomMode(); // First random trigger!
                    randomTimer = 0f;
                }
            }
            else
            {
                // Normal Random Intervals (Every X seconds)
                randomTimer += Time.deltaTime;

                if (timerText != null)
                {
                    float timeRemaining = Mathf.Max(0, randomInterval - randomTimer);
                    timerText.text = timeRemaining.ToString("F1");

                    if (timeRemaining <= 1.5f) timerText.color = Color.red;
                    else timerText.color = Color.white;
                }

                if (randomTimer >= randomInterval)
                {
                    randomTimer = 0f;
                    TriggerRandomMode();
                }
            }
        }
        else if (timerText != null)
        {
            timerText.text = ""; // Clear text if disabled
        }

        // --- 2. Manual Controls (These now ALWAYS work) ---
        if (Input.GetKeyDown(KeyCode.M))
        {
            isReversed = !isReversed;
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            isUpsideDown = !isUpsideDown;
            if (isUpsideDown)
            {
                isSideWays = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            isSideWays = !isSideWays;
            if (isSideWays)
            {
                isUpsideDown = false;
                omgIsLikeFez = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            omgIsLikeFez = !omgIsLikeFez;
            if (omgIsLikeFez)
            {
                isReversed = false;
                isUpsideDown = false;
                isSideWays = false;
            }
        }
    }

    private int GetCurrentStateIndex()
    {
        if (omgIsLikeFez) return 6;
        if (isSideWays) return isReversed ? 5 : 4;
        if (isUpsideDown) return isReversed ? 3 : 2;
        if (isReversed) return 1;

        return 0;
    }


    private void TriggerRandomMode()
    {
        int currentState = GetCurrentStateIndex();
        int nextState;

        // Roll a state between 0 and 6. 
        // Keep rolling if it's the exact same state we are currently in.
        do
        {
            nextState = UnityEngine.Random.Range(0, 7);
        }
        while (nextState == currentState);

        // First, reset all states to "Normal" base before applying new mode
        isReversed = false;
        isUpsideDown = false;
        isSideWays = false;
        omgIsLikeFez = false;

        // Apply the newly rolled state
        switch (nextState)
        {
            case 0:
                // Normal mode (Leave everything false)
                break;
            case 1:
                isReversed = true;    // Just Reversed
                break;
            case 2:
                isUpsideDown = true;  // Just UpsideDown
                break;
            case 3:
                isUpsideDown = true;  // N and M together
                isReversed = true;
                break;
            case 4:
                isSideWays = true;    // Just Sideways
                break;
            case 5:
                isSideWays = true;    // B and M together
                isReversed = true;
                break;
            case 6:
                omgIsLikeFez = true;  // Fez mode
                break;
        }
    }

    private void FixedUpdate()
    {
        Vector3 currentOffset = offSet;

        float targetYAngle = 0f;
        float targetZAngle = 0f;

        if (omgIsLikeFez)
        {
            currentOffset = sideViewOffset;
            targetYAngle = -90f; // Rotate 90 degrees to look at the side of the ship
        } else
        {
            if (isReversed)
            {
                currentOffset.z = reversedOffset;
                currentOffset.x = -offSet.x;
                targetYAngle = 180f;
            }

            if (isUpsideDown == true)
            {
                targetZAngle = 180f;
            } else if (isSideWays == true)
            {
                targetZAngle = -90f;
            }
        }

        Vector3 desiredPos = playerTrg.position + currentOffset;
        Vector3 smoothPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);

        float clampX;

        if (omgIsLikeFez)
        {
            clampX = smoothPos.x;
        }
        else
        {
            clampX = Mathf.Clamp(smoothPos.x, minPosX, maxPosX);
        }
        float clampY = Mathf.Clamp(smoothPos.y, minPosY, maxPosY);


        transform.position = new Vector3(clampX,clampY,smoothPos.z);

        Quaternion targetRotation = Quaternion.Euler(0, targetYAngle, targetZAngle);

        float angleDifference = Quaternion.Angle(transform.rotation, targetRotation);

        if (angleDifference > 1f)
        {
            isTransitioning = true;
        }
        else
        {
            isTransitioning = false;
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothSpeed);
    }
    public IEnumerator Shaking()
    {
        Vector3 start = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / duration);
            transform.position = start + UnityEngine.Random.insideUnitSphere * strength;
            yield return null;
        }

        transform.position = start;
    }

}
