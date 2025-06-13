using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public float SpawnRate = 5f;
    public float EnemySpeedMultiplier = 1;
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
        NavMeshAgent agent = Instantiate(EnemyObjects[EnemyIndex], SpawnPositions[SpawnIndex].position, SpawnPositions[SpawnIndex].rotation).GetComponent<NavMeshAgent>();
        agent.speed *= EnemySpeedMultiplier;
    }
}