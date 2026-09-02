// Controls idle movement of the menu's ship
using UnityEngine;

public class FloatIdle : MonoBehaviour
{
    [SerializeField] float bobAmplitude = 0.15f;
    [SerializeField] float bobSpeed = 0.8f;
    [SerializeField] float rollAmplitude = 2.5f;   
    [SerializeField] float pitchAmplitude = 1.5f;

    Vector3 startPos; Quaternion startRot;

    void Start() { startPos = transform.localPosition; startRot = transform.localRotation; }

    void Update()
    {
        float t = Time.time;
        float bob = Mathf.Sin(t * bobSpeed) * bobAmplitude
                  + Mathf.Sin(t * bobSpeed * 2.3f) * bobAmplitude * 0.3f;
        transform.localPosition = startPos + Vector3.up * bob;

        float roll  = Mathf.Sin(t * 0.6f) * rollAmplitude;
        float pitch = Mathf.Sin(t * 0.9f + 1.3f) * pitchAmplitude;
        transform.localRotation = startRot * Quaternion.Euler(pitch, 0f, roll);
    }
}