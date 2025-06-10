using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float SpawnRate = 5f;
    public GameObject[] EnemyObjects;
    public Transform[] SpawnPositions;

    private void Start()
    {
        StartCoroutine(KeepEnemiesSpawning());
    }

    IEnumerator KeepEnemiesSpawning()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(SpawnRate);

            SpawnObject();
        }
    }

    private void SpawnObject()
    {
        int EnemyIndex = Random.Range(0, EnemyObjects.Length);
        int SpawnIndex = Random.Range(0, SpawnPositions.Length);
        Instantiate(EnemyObjects[EnemyIndex], SpawnPositions[SpawnIndex].position, SpawnPositions[SpawnIndex].rotation);
    }
}