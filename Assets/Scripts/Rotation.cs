using UnityEngine;

public class Rotation : MonoBehaviour
{
    public Vector3 axis = Vector3.up;
    public float degreesPerSecond = 6f;

    void Update()
    {
        transform.Rotate(axis.normalized, degreesPerSecond * Time.deltaTime, Space.Self);
    }
}