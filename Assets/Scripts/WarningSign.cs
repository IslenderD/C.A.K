using UnityEngine;

public class HazardWarning : MonoBehaviour
{ 

    [Header("UI Settings")]
    public GameObject warningPrefab;
    private GameObject warningInstance;
    private RectTransform warningRect;


    [Header("Distance Settings")]
    public float showWarningDistance = 40f; // how far away

    // Cached references
    private Transform player;
    private Camera cam;
    private CameraControl camControl;
    private Canvas mainCanvas;

    private bool isPassed = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        cam = Camera.main;
        if (cam != null)
        {
            camControl = cam.GetComponent<CameraControl>();
        }

        mainCanvas = FindFirstObjectByType<Canvas>();
    }

    void Update()
    {
        if (isPassed || player == null || camControl == null)
        {
            CleanUpWarning();
            return;
        }

        bool isFrontalCamera = camControl.isReversed;

        float distanceToPlayer = transform.position.z - player.position.z; // how far the obstacle

        if (distanceToPlayer <= 0) //player dodge it
        {
            isPassed = true;
            CleanUpWarning();
            return;
        }

        if (distanceToPlayer <= showWarningDistance && isFrontalCamera) // in range
        {
            if (warningInstance == null) //ui not created
            {
                warningInstance = Instantiate(warningPrefab, mainCanvas.transform);
                warningRect = warningInstance.GetComponent<RectTransform>();
            }

            Vector3 fakePosition = new Vector3(transform.position.x, transform.position.y, player.position.z); //project x/y
            Vector3 screenPos = cam.WorldToScreenPoint(fakePosition);

            warningRect.position = screenPos;
        }
        else
        {
            CleanUpWarning();
        }
    }
    private void CleanUpWarning()
    {
        if (warningInstance != null)
        {
            Destroy(warningInstance);
        }
    }

    // Backup cleanup just in case the scene unloads
    void OnDestroy()
    {
        CleanUpWarning();
    }
}