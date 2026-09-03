using System;
using UnityEngine;
using System.Collections;

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

    public float reversedOffset = 15f;

    public bool omgIsLikeFez = false;
    public Vector3 sideViewOffset = new Vector3(20f, 0f, 0f);

    // Update is called once per frame
    void Update()
    {
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
                // Turn off other modes so they don't conflict with 2D
                isReversed = false;
                isUpsideDown = false;
                isSideWays = false;
            }
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
