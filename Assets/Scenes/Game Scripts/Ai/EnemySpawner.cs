using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public float SpawnRate = 5f;
    public float EnemySpeedMultiplier = 1;
    public List<GameObject> EnemyObjects = new List<GameObject>();
    public Transform[] SpawnPositions;

    float time_elapsed = 0;
    private void Update()
    {
        if (time_elapsed < SpawnRate)
        {
            time_elapsed += Time.deltaTime;
        }
        else
        {
            time_elapsed = 0;
            SpawnObject();
        }
    }

    private void SpawnObject()
    {
        int EnemyIndex = Random.Range(0, EnemyObjects.Count);
        Debug.Log(EnemyIndex);

        int SpawnIndex = Random.Range(0, SpawnPositions.Length);
        NavMeshAgent agent = Instantiate(EnemyObjects[EnemyIndex],
            SpawnPositions[SpawnIndex].position, SpawnPositions[SpawnIndex].rotation).GetComponent<NavMeshAgent>();
        agent.speed *= EnemySpeedMultiplier;
    }
}