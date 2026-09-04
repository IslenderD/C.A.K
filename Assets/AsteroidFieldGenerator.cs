using UnityEngine;

public class AsteroidFieldGenerator : MonoBehaviour
{
    [Header("Asteroid Models")]
    public GameObject[] asteroidPrefabs;

    [Header("Amount")]
    public int corridorAsteroids = 30;
    public int outsideAsteroids = 70;

    [Header("Flight Corridor")]
    public Vector3 corridorSize = new Vector3(12f, 8f, 100f);

    [Header("Entire Asteroid Field")]
    public Vector3 outerSize = new Vector3(40f, 25f, 100f);

    [Header("Asteroid Size")]
    public float minimumScale = 0.5f;
    public float maximumScale = 2f;

    [Header("Randomness")]
    public int seed = 12345;


    [ContextMenu("Generate Asteroids")]
    public void GenerateAsteroids()
    {
        ClearAsteroids();

        if (asteroidPrefabs == null || asteroidPrefabs.Length == 0)
        {
            Debug.LogWarning("No asteroid prefabs have been assigned.");
            return;
        }

        Random.InitState(seed);

        GameObject container = new GameObject("Generated Asteroids");
        container.transform.SetParent(transform);
        container.transform.localPosition = Vector3.zero;

        // Asteroids INSIDE the flight corridor
        for (int i = 0; i < corridorAsteroids; i++)
        {
            Vector3 position = RandomPointInside(corridorSize);
            CreateAsteroid(position, container.transform);
        }

        // Asteroids OUTSIDE the flight corridor,
        // but still inside the larger asteroid field.
        for (int i = 0; i < outsideAsteroids; i++)
        {
            Vector3 position = RandomPointOutsideCorridor();
            CreateAsteroid(position, container.transform);
        }
    }


    Vector3 RandomPointInside(Vector3 size)
    {
        return new Vector3(
            Random.Range(-size.x / 2f, size.x / 2f),
            Random.Range(-size.y / 2f, size.y / 2f),
            Random.Range(-size.z / 2f, size.z / 2f)
        );
    }


    Vector3 RandomPointOutsideCorridor()
    {
        Vector3 point;

        do
        {
            point = RandomPointInside(outerSize);
        }
        while (
            Mathf.Abs(point.x) < corridorSize.x / 2f &&
            Mathf.Abs(point.y) < corridorSize.y / 2f
        );

        return point;
    }


    void CreateAsteroid(Vector3 localPosition, Transform parent)
    {
        GameObject prefab =
            asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];

        GameObject asteroid = Instantiate(prefab, parent);

        asteroid.transform.localPosition = localPosition;

        asteroid.transform.localRotation = Random.rotation;

        float randomScale =
            Random.Range(minimumScale, maximumScale);

        asteroid.transform.localScale *= randomScale;
    }


    [ContextMenu("Clear Asteroids")]
    public void ClearAsteroids()
    {
        Transform oldContainer = transform.Find("Generated Asteroids");

        if (oldContainer == null)
            return;

        if (Application.isPlaying)
            Destroy(oldContainer.gameObject);
        else
            DestroyImmediate(oldContainer.gameObject);
    }


    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;

        // Inner flight corridor
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Vector3.zero, corridorSize);

        // Entire asteroid field
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Vector3.zero, outerSize);
    }
}