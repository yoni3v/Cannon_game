using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CanonShooter
{
    [System.Serializable]
    public struct WaveSetting
    {
        public int[] m_EnemyTypes;
    }
    public class SpawnControl : MonoBehaviour
    {
        private bool m_DelayingSpawn = true;

        public GameObject[] m_EnemyPrefabs;
        public GameObject[] m_SpawnPoints;
        [SerializeField]
        protected GameObject m_SpawnParticlePrefab;

        [HideInInspector]
        public List<GameObject> m_Enemies;

        [HideInInspector]
        public int SpawnCounter = 0;
        [HideInInspector]
        public int TotalSpawnCount = 0;
        [HideInInspector]
        public int CurrentEnemyCount = 0;
        [HideInInspector]
        public bool KeepSpawnning = true;
        [HideInInspector]
        public int TotalKillCount = 0;

        public static SpawnControl Current;

        public int SpawnWaveNum = 0;
        float SpawnDelay = 2;
        float SpawnDistance = 20;

        public int[] LevelSpawnCount;

        public WaveSetting[] m_Waves;
        void Awake()
        {
            Current = this;
            m_Enemies = new List<GameObject>();

            //GameObject[] objs = GameObject.FindGameObjectsWithTag("SpawnPoint");
            //m_SpawnPoints = objs;
        }
        // Start is called before the first frame update
        void Start()
        {
            m_DelayingSpawn = false;
            SpawnCounter = 0;
            CurrentEnemyCount = 0;
            KeepSpawnning = true;
            TotalSpawnCount = 20;
            SpawnDelay = 1.2f;
            SpawnWaveNum = 0;
            SpawnDistance = 20;
        }

        public void StartSpawnLoop()
        {
            StartCoroutine(Co_SpawnLoop());
        }

        IEnumerator Co_SpawnLoop()
        {
            while (true)
            {
                float distanceOffset = Random.Range(0f, 5f);
                Vector3 pos = Quaternion.Euler(0, Random.Range(0, 360), 0) * new Vector3(0, 0, SpawnDistance + distanceOffset);

                GameObject obj1 = Instantiate(m_SpawnParticlePrefab);
                obj1.transform.position = pos;
                Destroy(obj1, 3);

                yield return new WaitForSeconds(.2f);

                GameObject obj = Instantiate(m_EnemyPrefabs[m_Waves[SpawnWaveNum].m_EnemyTypes[SpawnCounter]]);
                obj.transform.position = pos;
                m_Enemies.Add(obj);
                AddEnemy();

                SpawnCounter++;
                if (SpawnCounter > m_Waves[SpawnWaveNum].m_EnemyTypes.Length - 1)
                {
                    while (CurrentEnemyCount > 0)
                        yield return null;

                    SpawnCounter = 0;
                    SpawnWaveNum++;
                    if (SpawnWaveNum > 9)
                        SpawnWaveNum = 9;
                    SpawnDelay -= .1f;
                    SpawnDelay = Mathf.Clamp(SpawnDelay, .1f, 2);
                    SpawnDistance -= 1;
                    SpawnDistance = Mathf.Clamp(SpawnDistance, 10, 30);
                    yield return new WaitForSeconds(4f);
                }

                yield return new WaitForSeconds(SpawnDelay);
            }
        }


        public void StopAllEnemies()
        {
            foreach (GameObject obj in m_Enemies)
            {
                EnemyMovement move = obj.GetComponent<EnemyMovement>();
                move.m_IsMoving = false;
            }
        }
        public void AddEnemy()
        {

            CurrentEnemyCount++;

        }

        public void RemoveEnemy(GameObject enemy)
        {
            m_Enemies.Remove(enemy);
            CurrentEnemyCount--;
            TotalKillCount++;
        }
    }
}
