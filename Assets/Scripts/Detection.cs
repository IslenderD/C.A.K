using UnityEngine;

public class Detection : MonoBehaviour
{
    [Header("Detection Settings")]
    [Tooltip("How far ahead to project the detection plane")]
    public float detectionDistance = 50f;

    [Tooltip("Size of the detection plane (Width, Height, Thickness)")]
    public Vector3 planeSize = new Vector3(3f, 3f, 0.1f);

    [Tooltip("The tag assigned to the asteroids")]
    public string hazardTag = "enemy";

    [Header("UI Settings")]
    [Tooltip("Drag the UI GameObject for the Top Warning here")]
    public GameObject topWarningUI;

    private CameraControl camControl;

    // Buffer to store what the BoxCast hits (prevents lag from memory allocation)
    private RaycastHit[] hitResults = new RaycastHit[10];

    void Start()
    {
        // Get the CameraControl from the main camera
        if (Camera.main != null)
        {
            camControl = Camera.main.GetComponent<CameraControl>();
        }

        // Ensure the warning is hidden at start
        if (topWarningUI != null)
        {
            topWarningUI.SetActive(false);
        }
    }

    void Update()
    {
        if (camControl == null || topWarningUI == null) return;

        // ONLY trigger if the camera is facing the front of the ship (M or N+M)
        if (camControl.isReversed && !camControl.omgIsLikeFez)
        {
            CheckForHazards();
        }
        else
        {
            // If we are looking from behind (or Fez mode), turn off the warning
            if (topWarningUI.activeSelf)
            {
                topWarningUI.SetActive(false);
            }
        }
    }

    void CheckForHazards()
    {
        // BoxCastNonAlloc sweeps a box forward and fills our hitResults array with everything it touches.
        int hitCount = Physics.BoxCastNonAlloc(
            transform.position,
            planeSize / 2,              // BoxCast uses half-extents (size divided by 2)
            Vector3.forward,            // Cast strictly down the Z-axis
            hitResults,
            Quaternion.identity,
            detectionDistance,
            Physics.AllLayers,          // Check everything...
            QueryTriggerInteraction.Collide // Allow it to detect triggers if your asteroids use "IsTrigger"
        );

        bool hazardAhead = false;

        // Loop through everything the box touched this frame
        for (int i = 0; i < hitCount; i++)
        {
            // If ANY of the things we hit have the "Enemy" tag, we are in danger!
            if (hitResults[i].collider.CompareTag(hazardTag))
            {
                hazardAhead = true;
                break; // Stop checking, we already know we're going to hit something
            }
        }

        // Toggle UI based on what we found
        if (hazardAhead)
        {
            if (!topWarningUI.activeSelf) topWarningUI.SetActive(true);
        }
        else
        {
            if (topWarningUI.activeSelf) topWarningUI.SetActive(false);
        }
    }

    // This draws a red box in the Scene view so you can visually tune your detection plane!
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        // Draw the volume being checked
        Gizmos.DrawWireCube(transform.position + (Vector3.forward * (detectionDistance / 2)),
                            new Vector3(planeSize.x, planeSize.y, detectionDistance));
    }
}