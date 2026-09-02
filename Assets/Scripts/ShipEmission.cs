using UnityEngine;
using YourNamespace;

public class ShipEmission : MonoBehaviour
{
    [Header("Main VFX")]
    [SerializeField] private VFX_FireController fullOpaqueFire;

    [Header("Fast Movement VFX")]
    [SerializeField] private TrailRenderer[] wingTrails;

    [Header("Fire Intensity")]
    [SerializeField] private float normalIntensity = 1f;
    [SerializeField] private float boostIntensity = 2f;
    [SerializeField] private float brakeIntensity = 0.5f;

    private void Start()
    {
        EmitNorm();
    }

    public void EmitNorm()
    {
        // Normal engine fire
        fullOpaqueFire.SetFireIntensity(normalIntensity);

        // No wing trails
        SetWingTrails(false);
    }

    public void EmitBoost()
    {
        // Bigger/stronger engine fire
        fullOpaqueFire.SetFireIntensity(boostIntensity);

        // Enable wing trails
        SetWingTrails(true);
    }

    public void EmitBrake()
    {
        // Smaller/weaker engine fire
        fullOpaqueFire.SetFireIntensity(brakeIntensity);

        // No wing trails
        SetWingTrails(false);
    }

    private void SetWingTrails(bool enabled)
    {
        foreach (TrailRenderer trail in wingTrails)
        {
            if (trail != null)
                trail.emitting = enabled;
        }
    }
}