using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public class Orb_spawner : MonoBehaviour
{
    [SerializeField] GameObject Dead_orb;

    private void Start()
    {
        HealthSystem healthSystem = GetComponent<HealthSystem>();

        healthSystem.OnDead.AddListener(SpawnOrb);
    }

    private void SpawnOrb()
    {
        Instantiate(Dead_orb, transform.position, Quaternion.identity);
    }
}