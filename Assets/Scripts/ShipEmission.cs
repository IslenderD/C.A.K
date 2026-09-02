using UnityEngine;
using YourNamespace;

public class ShipEmission : MonoBehaviour
{
    [Header("Main Engine VFX")]
    [SerializeField] private VFX_FireController fullOpaqueFire;

    [Header("Wing Speed Trails")]
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

        // Disable wing trails
        SetWingTrails(false);
    }

    public void EmitBoost()
    {
        // Stronger engine fire
        fullOpaqueFire.SetFireIntensity(boostIntensity);

        // Enable wing trails
        SetWingTrails(true);
    }

    public void EmitBrake()
    {
        // Weaker engine fire
        fullOpaqueFire.SetFireIntensity(brakeIntensity);

        // Disable wing trails
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