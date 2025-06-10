using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CanonShooter
{
    public class PlayerCanon : MonoBehaviour
    {
        public AimAtPointer m_Aim;
        public GameObject[] m_BulletPrefabs;
        public GameObject m_BulletFirePrefab1;
        public Transform m_FirePoint;
        public Animator m_Animator;
        public GameObject[] m_DamagePoints;
        public bool m_CanShoot = true;
        public static PlayerCanon m_Main;

        public bool m_IsReloading = false;
        public int m_MagAmmo = 10;
        public int m_WeaponNum = 0;
        public int m_RocketAmmo = 0;

        public bool m_IsDead = false;

        public bool m_ShieldActive;
        public GameObject m_ShieldObject;

        public float m_ShieldTime = 0;

        void Awake()
        {
            m_Main = this;
        }
        // Start is called before the first frame update
        void Start()
        {
            m_CanShoot = true;
            m_IsDead = false;
            m_IsReloading = false;
            m_MagAmmo = 10;
            m_RocketAmmo = 0;
            m_WeaponNum = 0;

            m_ShieldActive = false;
            m_ShieldObject.SetActive(false);
            m_ShieldTime = 0;
        }

        // Update is called once per frame
        void Update()
        {
            DamageControl damage = GetComponent<DamageControl>();
            for (int i = 0; i < 3; i++)
            {
                m_DamagePoints[i].gameObject.SetActive((i < (int)damage.Damage));
            }

            if (m_CanShoot && !m_IsReloading && m_MagAmmo > 0)
            {
                if (Input.GetMouseButtonDown(0))
                {

                    Vector3 dir = m_Aim.m_AimPoint - transform.position;
                    dir.y = 0;
                    GameObject obj = Instantiate(m_BulletPrefabs[m_WeaponNum]);
                    obj.transform.position = m_FirePoint.position;
                    obj.transform.forward = dir;

                    GameObject obj1 = Instantiate(m_BulletFirePrefab1);
                    obj1.transform.position = m_FirePoint.position;
                    Destroy(obj1, 3);

                    CameraControl.m_Main.StartShake(.2f, .2f);
                    m_Animator.SetTrigger("shoot");

                    UI_HUD.m_Main.ReticleEff();

                    if (m_WeaponNum == 0)
                    {
                        m_MagAmmo--;
                        if (m_MagAmmo <= 0)
                        {
                            m_MagAmmo = 0;
                            StartCoroutine(Co_Reload());
                        }
                    }
                    else if (m_WeaponNum == 1)
                    {
                        m_RocketAmmo--;
                        if (m_RocketAmmo <= 0)
                        {
                            m_RocketAmmo = 0;
                            m_WeaponNum = 0;

                        }
                    }
                }
            }

            if (!m_IsDead)
            {
                if (damage.IsDead)
                {
                    m_IsDead = true;
                    m_CanShoot = false;
                    AimAtPointer.m_Main.m_AimEnable = false;
                    GameControl.m_Main.HandleGameOver();
                }
            }

            if (m_ShieldActive)
            {
                m_ShieldTime -= Time.deltaTime;
                if (m_ShieldTime <= 0)
                {
                    m_ShieldTime = 0;
                    m_ShieldActive = false;
                    m_ShieldObject.SetActive(false);
                }
            }
        }

        public void GiveRocket()
        {
            m_WeaponNum = 1;
            m_RocketAmmo += 4;
        }

        public void ActivateShield()
        {
            m_ShieldTime = 10;
            m_ShieldActive = true;
            m_ShieldObject.SetActive(true);
        }
        IEnumerator Co_Reload()
        {
            m_IsReloading = true;
            UI_HUD.m_Main.m_ReloadText.gameObject.SetActive(true);
            yield return new WaitForSeconds(3);
            m_IsReloading = false;
            UI_HUD.m_Main.m_ReloadText.gameObject.SetActive(false);
            m_MagAmmo = 10;
        }
        public void HandleDamage()
        {

        }
    }
}
