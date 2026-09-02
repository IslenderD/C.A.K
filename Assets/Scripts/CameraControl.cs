using System;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] Transform playerTrg;

    public float smoothSpeed = 0.1f;
    public Vector3 offSet;

    Vector3 Velocity = Vector3.zero;

    [SerializeField] float minPosX, maxPosX;
    [SerializeField] float minPosY, maxPosY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector3 desiredPos = playerTrg.position + offSet;
        Vector3 smoothPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);

        transform.position = smoothPos;

        transform.position = new Vector3(Mathf.Clamp(smoothPos.x, minPosX,maxPosX),
            Mathf.Clamp(smoothPos.y, minPosY,maxPosY),
            transform.position.z);
    }
}
