using UnityEngine;

/// Randomly fills hand-placed spawn points with trap prefabs at level start,
/// so each run has a different trap layout.
public class TrapSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] trapPrefabs;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField, Range(0f, 1f)] private float spawnChance = 0.7f;

    void Start()
    {
        if (trapPrefabs == null || trapPrefabs.Length == 0 || spawnPoints == null)
            return;

        foreach (Transform point in spawnPoints)
        {
            if (point == null)
                continue;

            if (Random.value < spawnChance)
            {
                GameObject prefab = trapPrefabs[Random.Range(0, trapPrefabs.Length)];
                Instantiate(prefab, point.position, Quaternion.identity, transform);
            }
        }
    }
}
