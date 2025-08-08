using FirstGearGames.SmoothCameraShaker;
using System.Collections;
using UnityEngine;

public class Player_Canon_Modded : MonoBehaviour
{
    public static bool TakeInput = true;
    public static bool Autoplay = false;

    [Header("Settings")]
    public float projectileSpeed = 1;
    public float ProjectileSpeedMultiplier = 1f;
    public float ShootingIntensity = 1.3f;
    public float ShootingEffectDuration = 0.4f;
    public Vector3 RotationOffset = Vector3.zero;
    public ShakeData shakeData;

    [Header("Transform References")]
    public CannonOBJ[] Cannon_Objects;

    public Transform _base_cannon;
    [SerializeField] Transform _shoot_point;
    [SerializeField] ParticleSystem ShootingParticle;
    
    [SerializeField] GameObject projectile;
    [SerializeField] ParticleSystem Impact_Particle;

    public GameScoreManager scoreManager;
    public Toolbox_UI toolbox;

    #region Modules

    public void ChangeProjectile(GameObject new_projectile, float NewSpeed, Vector3 _RotationOffset)
    {
        projectile = new_projectile;
        projectileSpeed = NewSpeed;
        RotationOffset = _RotationOffset;
    }

    public void ChangeImpactParticle(ParticleSystem new_particle)
    {
        Impact_Particle = new_particle;
    }

    public void ChangeCannonModel(string new_cannon)
    {
        GameObject spawnedUnit = null;
        Transform shootpoint = null;
        Transform movement_part = null;

        foreach (var item in Cannon_Objects)
        {
            if (item.Obj.name == new_cannon)
            {
                spawnedUnit = item.Obj.gameObject;
                shootpoint = item.Shootpoint;
                movement_part = item.MovementPoint;
                break;
            }
        }

        if (shootpoint != null)
        {
            _shoot_point = shootpoint;
        }
        else
        {
            Debug.LogError("The shootpoint is not found in the spawned unit");
        }

        if (movement_part != null)
        {
            _base_cannon = movement_part;
        }
        else
        {
            Debug.LogError("The movement_part is not found in the spawned unit");
        }

        foreach (CannonOBJ item in Cannon_Objects)
        {
            item.Obj.gameObject.SetActive(false);
        }

        spawnedUnit.SetActive(true);
    }

    #endregion

    #region Basic Mechanics

    private void Update()
    {
        if (!TakeInput || Autoplay || Time.timeScale == 0)
        {
            return;
        }

        //set the cursor objects position to cursor space
        Vector3 cursor_position = CursorPositionInWorldSpace.instance.output;

        //Rotate the canon head towards the cursor
        float Y_axis_angle = Vector3.SignedAngle(transform.forward, cursor_position, Vector3.up);
        _base_cannon.eulerAngles = new Vector3(0, Y_axis_angle, 0);

        //shoot canon
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot(cursor_position);
        }
    }

    public void Shoot(Vector3 shootpoint)
    {
        StartCoroutine(ShootingEffectBase(ShootingEffectDuration, ShootingIntensity));

        if (GameUI_Manager.AllowRandomProjectiles)
        {
            int TotalNumberOfItems = toolbox.items.Count;
            int RandomItem = Random.Range(0, TotalNumberOfItems);
            item item = toolbox.items[RandomItem];

            ChangeProjectile(item.Projectile_Object, item.speed, item.rotation_offset);
        }

        var projectile_obj = Instantiate(projectile, _shoot_point.position, Quaternion.Euler(RotationOffset));

        StartCoroutine(HandleProjectile(projectile_obj, shootpoint));
    }

    IEnumerator HandleProjectile(GameObject projectile_obj, Vector3 final_position)
    {
        float ProjectileDistance = (final_position - projectile_obj.transform.position).magnitude;

        while (ProjectileDistance > .4f)
        {
            //face the projectile towards the final position
            projectile_obj.transform.LookAt(final_position);

            //translate it towards the destination                                           [TRANSLATION]
            projectile_obj.transform.position += projectile_obj.transform.forward * projectileSpeed * ProjectileSpeedMultiplier * Time.deltaTime;

            //Do some damage checking                                                        [DAMAGE LOGIN]
            Collider[] enemies = Physics.OverlapSphere(projectile_obj.transform.position, 1);
            foreach (Collider enemy in enemies)
            {
                iDamageable damageObj = enemy.GetComponent<iDamageable>();

                if (damageObj != null)
                {
                    damageObj.OnDamage(200);
                    scoreManager.RegisterKill();

                    //Play the particle                                                                   [VISUALS]
                    var _Destroy = Instantiate(Impact_Particle, projectile_obj.transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
                    _Destroy.Play();

                    //Set dirty the spawned projectile                                                   [DESTROY]
                    Destroy(_Destroy.gameObject, _Destroy.main.duration);

                    //Set dirty the spawned projectile                                                   [DESTROY]
                    Destroy(projectile_obj);

                    yield break;                                                                                // [BREAKS THE CURRENT ROUTINE]
                }
            }

            //recalculate the distance                                                       [RECALCULATION]
            ProjectileDistance = (final_position - projectile_obj.transform.position).magnitude;

            //return null on worker to avoid the main thread from stoping                    [AVOID CRASH]
            yield return null;
        }

        //Set dirty the spawned projectile                                                   [DESTROY]
        Destroy(projectile_obj, .2f);

        //Play the particle                                                                   [VISUALS]
        var particle = Instantiate(Impact_Particle, final_position, Quaternion.identity).GetComponent<ParticleSystem>();
        particle.Play();

        //destroy particle as well when its done
        Destroy(particle.gameObject, particle.main.duration);
    }

    IEnumerator ShootingEffectBase(float duration, float intensity)
    {
        float _time_ref = 0;
        ShootingParticle.Play();
        CameraShakerHandler.Shake(shakeData);

        while (_time_ref < duration)
        {
            _time_ref += Time.deltaTime;

            Vector3 Initial = new Vector3(0, _base_cannon.transform.eulerAngles.y, _base_cannon.transform.eulerAngles.z);
            Vector3 Final = new Vector3(-20, _base_cannon.transform.eulerAngles.y, _base_cannon.transform.eulerAngles.z);
            float ratio = _time_ref / duration;
            float Control = 0;

            if (ratio >= 0.5f)
            {
                Control = 1 - ((ratio - 0.5f) * 2);
            }
            else
            {
                Control = ratio * 2;
            }
            
            _base_cannon.transform.eulerAngles = Vector3.Slerp(Initial, Final * intensity, Control);

            yield return null;
        }
    }

    #endregion
}

[System.Serializable]
public class CannonOBJ
{
    public Transform Obj;
    public Transform Shootpoint;
    public Transform MovementPoint;
}