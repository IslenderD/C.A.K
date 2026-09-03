using UnityEngine;

public class HazardWarning : MonoBehaviour
{
    public bool doIWarn = false;

    [Header("UI Settings")]
    public GameObject warningPrefab;
    private GameObject warningInstance;
    private RectTransform warningRect;


    [Header("Distance Settings")]
    public float showWarningDistance = 40f; // how far away

    // Cached references
    private Transform player;
    private Camera cam;
    private Canvas mainCanvas;

    private bool isPassed = false;

    void Start()
    {
        if (!doIWarn)
            return;

        player = GameObject.FindGameObjectWithTag("Player").transform;
        cam = Camera.main;
        mainCanvas = FindFirstObjectByType<Canvas>();
    }

    void Update()
    {
        if (!doIWarn || isPassed || player == null)
            return;
        float distanceToPlayer = transform.position.z - player.position.z; // how far the obstacle

        if (distanceToPlayer <= showWarningDistance && distanceToPlayer > 0) // in range
        {
            if (warningInstance == null) //ui not created
            {
                warningInstance = Instantiate(warningPrefab, mainCanvas.transform);
                warningRect = warningInstance.GetComponent<RectTransform>();
            }

            Vector3 fakePosition = new Vector3(transform.position.x, transform.position.y, player.position.z); //project x/y
            Vector3 screenPos = cam.WorldToScreenPoint(fakePosition);

            warningRect.position = screenPos;
        } else if (distanceToPlayer <= 0) //player dodge it
        {
            isPassed = true;
            if (warningInstance != null) // Clean up
            {
                Destroy(warningInstance);
            }
        }
    }

    // Backup cleanup just in case the scene unloads
    void OnDestroy()
    {
        if (warningInstance != null)
        {
            Destroy(warningInstance);
        }
    }
}