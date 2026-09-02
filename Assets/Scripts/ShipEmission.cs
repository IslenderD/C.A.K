using UnityEngine;

public class ShipEmission : MonoBehaviour
{

    public Transform emitter;
    public TrailRenderer[] wingTrails;
    [SerializeField] bool emitTrails;

    [SerializeField] float scaleMultiplier = 1.5f;
    [SerializeField] float scaleSpeed = 5f;

    Vector3 defaultScale, targetScale;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        defaultScale = emitter.localScale;
        targetScale = defaultScale;
    }

    // Update is called once per frame
    void Update()
    {
        targetScale = defaultScale * scaleMultiplier;
        emitTrails = true;
    }

    public void EmitBrake()
    {
        targetScale = defaultScale * 0.5f;
    }
}
