using UnityEngine;

public class ShakyText : MonoBehaviour
{
    public float amount = 3f;
    public float speed = 12f;
    public bool smooth = true;

    RectTransform rt;
    Vector2 homeUI;
    Vector3 homeWorld;
    float seed;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        if (rt != null) homeUI = rt.anchoredPosition;
        else homeWorld = transform.localPosition;
        seed = Random.value * 100f;
    }

    void Update()
    {
        Vector2 offset;
        if (smooth)
        {
            float t = Time.unscaledTime * speed;
            offset = new Vector2(Mathf.PerlinNoise(seed, t) - 0.5f,
                                 Mathf.PerlinNoise(seed + 37f, t) - 0.5f) * 2f;
        }
        else offset = Random.insideUnitCircle;

        if (rt != null) rt.anchoredPosition = homeUI + offset * amount;
        else transform.localPosition = homeWorld + (Vector3)(offset * amount);
    }
}