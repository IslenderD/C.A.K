using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class TitleAnimation : MonoBehaviour
{
    [Header("Fade in")]
    public float startDelay = 0.2f;
    public float fadeDuration = 0.35f;      // per letter
    public float letterStagger = 0.06f;
    public float riseAmount = 12f;

    [Header("Idle flips")]
    public bool flipsEnabled = true;
    public Vector2 flipInterval = new Vector2(0.6f, 2.2f);
    public float flipDuration = 0.55f;
    [Range(0f, 1f)] public float chanceFlipVertical = 0.25f;

    [Header("Idle wobble")]
    public float wobbleAmount = 1.2f;
    public float wobbleSpeed = 6f;

    TMP_Text text;
    float t0, nextFlip;
    int count;
    float[] flipT;
    bool[] flipVertical;

    void Awake() => text = GetComponent<TMP_Text>();

    void OnEnable() => Rebuild();

    void Rebuild()
    {
        text.ForceMeshUpdate();
        count = text.textInfo.characterCount;
        flipT = new float[count];
        flipVertical = new bool[count];
        for (int i = 0; i < count; i++) flipT[i] = -1f;
        t0 = Time.unscaledTime + startDelay;
        nextFlip = Random.Range(flipInterval.x, flipInterval.y);
    }

    void Update()
    {
        text.ForceMeshUpdate();
        var info = text.textInfo;
        if (info.characterCount != count) { Rebuild(); return; }

        float now = Time.unscaledTime;
        float dt = Time.unscaledDeltaTime;
        bool introDone = now > t0 + count * letterStagger + fadeDuration;

        if (flipsEnabled && introDone)
        {
            nextFlip -= dt;
            if (nextFlip <= 0f)
            {
                StartFlip();
                nextFlip = Random.Range(flipInterval.x, flipInterval.y);
            }
        }

        for (int i = 0; i < info.characterCount; i++)
        {
            if (!info.characterInfo[i].isVisible) continue;

            int mat = info.characterInfo[i].materialReferenceIndex;
            int vi  = info.characterInfo[i].vertexIndex;
            var verts = info.meshInfo[mat].vertices;
            var cols  = info.meshInfo[mat].colors32;

            // fade + rise
            float a = Mathf.Clamp01((now - t0 - i * letterStagger) / fadeDuration);
            a = a * a * (3f - 2f * a);
            Vector3 offset = Vector3.down * (riseAmount * (1f - a));

            // idle wobble, only once the letter has landed
            if (introDone && wobbleAmount > 0f)
            {
                float w = now * wobbleSpeed + i * 0.7f;
                offset += new Vector3(Mathf.PerlinNoise(w, 0f) - 0.5f,
                                      Mathf.PerlinNoise(0f, w) - 0.5f, 0f) * wobbleAmount * 2f;
            }

            // flip
            float angle = 0f;
            if (flipT[i] >= 0f)
            {
                flipT[i] += dt;
                float p = flipT[i] / flipDuration;
                if (p >= 1f) { flipT[i] = -1f; p = 1f; }
                angle = (p * p * (3f - 2f * p)) * 360f;
            }

            Vector3 centre = (verts[vi] + verts[vi + 2]) * 0.5f;
            Quaternion q = Quaternion.Euler(flipVertical[i] ? angle : 0f,
                                            flipVertical[i] ? 0f : angle, 0f);
            Matrix4x4 m4 = Matrix4x4.TRS(Vector3.zero, q, Vector3.one);

            for (int v = 0; v < 4; v++)
            {
                verts[vi + v] = m4.MultiplyPoint3x4(verts[vi + v] - centre) + centre + offset;
                var c = cols[vi + v];
                c.a = (byte)(a * 255);
                cols[vi + v] = c;
            }
        }

        for (int m = 0; m < info.meshInfo.Length; m++)
        {
            info.meshInfo[m].mesh.vertices = info.meshInfo[m].vertices;
            info.meshInfo[m].mesh.colors32 = info.meshInfo[m].colors32;
            text.UpdateGeometry(info.meshInfo[m].mesh, m);
        }
    }

    void StartFlip()
    {
        var info = text.textInfo;
        for (int attempt = 0; attempt < 8; attempt++)
        {
            int i = Random.Range(0, Mathf.Min(count, info.characterCount));
            if (!info.characterInfo[i].isVisible || flipT[i] >= 0f) continue;
            flipT[i] = 0f;
            flipVertical[i] = Random.value < chanceFlipVertical;
            return;
        }
    }
}