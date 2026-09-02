using UnityEngine;

public class ShipEmission : MonoBehaviour
{

    public Transform emitter;
    public TrailRenderer[] wingTrails;
    [SerializeField] bool emitTrails;

    [SerializeField] float scaleMultiplier = 1.5f;
    [SerializeField] float scaleSpeed = 5f;

    Vector3 defaultScale, targetScale;

    void Start()
    {
        defaultScale = emitter.localScale;
        targetScale = defaultScale;
    }

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
