using UnityEngine;

public class PlayerAutoPlayer : MonoBehaviour
{
    public bool AutoPlay = false;
    public float TargetSwitchDelay = 1f;
    public float EnemyDetectionRange = 10;
    public LayerMask EnemyMask;
    bool canShoot = true;

    Player_Canon_Modded player;

    private void Start()
    {
        player = GetComponent<Player_Canon_Modded>();
    }

    private void Update()
    {
        Player_Canon_Modded.Autoplay = AutoPlay;

        if (AutoPlay && canShoot)
        {
            //List of all the enemies in range
            Collider[] colliders = Physics.OverlapSphere(transform.position, EnemyDetectionRange, EnemyMask);

            //This vector will store the position of the enemy
            Vector3 shootposition = Vector3.zero;

            //Varaiable used to detect the nearest enemy
            float Nearestdistance = EnemyDetectionRange;
            foreach (Collider collider in colliders)
            {
                float EnemyDistance = (collider.transform.position - transform.position).magnitude;

                if (Nearestdistance > EnemyDistance)
                {
                    shootposition = collider.transform.position;
                    Nearestdistance = EnemyDistance;
                }
            }

            if (shootposition != Vector3.zero)
            {
                player._base_cannon.transform.LookAt(shootposition);
                player.Shoot(shootposition);
                canShoot = false;
                Invoke(nameof(ResetShoot), TargetSwitchDelay);
            }
        }
    }

    private void ResetShoot()
    {
        canShoot = true;
    }

    //for the editor
    private void OnDrawGizmosSelected()
    {
        if (AutoPlay)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, EnemyDetectionRange);
        }
    }
}