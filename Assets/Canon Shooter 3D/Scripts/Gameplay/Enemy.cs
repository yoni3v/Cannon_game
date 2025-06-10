using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CanonShooter
{
    public class Enemy : MonoBehaviour
    {
        protected DamageControl m_DamageControl;
        [SerializeField]
        protected GameObject m_DeathParticlePrefab;
        [SerializeField]
        protected GameObject m_SpawnParticlePrefab;
        [HideInInspector]
        public Vector3 MoveDirection;
        [HideInInspector]
        public Vector3 InitPosition;

        public Transform ShakeBase;
        public Transform m_RotationBase;
        public Transform m_FirePoint;

        public GameObject[] m_ItemPrefabs;

        public GameObject m_RagdollPrefab;

        [HideInInspector]
        public bool m_FacePlayer = false;

        //AI
        bool m_CanChangeTargetPosition = true;
        bool m_FrontClear = true;

        bool m_CanDamage = true;

        public bool m_StartAttack = false;
        public float m_StartWalkDistance = 10;

        [HideInInspector]
        public bool m_IsDead = false;

        [HideInInspector]
        public bool m_Alerted = false;

        public Animator m_Animator;

        public int m_ItemDropCount = 1;
        // Start is called before the first frame update
        void Start()
        {
            m_CanDamage = true;
            m_DamageControl = GetComponent<DamageControl>();

            InitPosition = transform.position;

            EnemyMovement movement = GetComponent<EnemyMovement>();
            movement.m_MoveSpeed = Random.Range(.8f, 1.2f);
        }

        // Update is called once per frame
        void Update()
        {
            EnemyMovement movement = GetComponent<EnemyMovement>();
            if (movement.m_IsMoving)
            {
                Vector3 targetPos = PlayerCanon.m_Main.transform.position;
                Vector3 dir = targetPos - transform.position;
                dir.y = 0;
                if (dir.magnitude < 2)
                {
                    movement.m_IsMoving = false;
                    DamageControl damage = PlayerCanon.m_Main.GetComponent<DamageControl>();
                    damage.ApplyDamage(1, Vector3.up, 1);

                    m_DamageControl.ApplyDamage(m_DamageControl.MaxDamage, Vector3.up, 1);
                    CameraControl.m_Main.StartShake(.5f, 1);
                    PlayerHitEffect.m_Main.StartGlow();
                }
                else if (PlayerCanon.m_Main.m_ShieldActive)
                {
                    if (dir.magnitude < 4)
                    {
                        movement.m_IsMoving = false;
                        m_DamageControl.ApplyDamage(m_DamageControl.MaxDamage, Vector3.up, 1);
                    }
                }
            }
            //AI
            // Vector3 forward = Vector3.zero - transform.position;
            // forward.y = 0;
            // Quaternion rotation = Quaternion.LookRotation(forward);
            // transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 10 * Time.deltaTime);

            // Vector3 axis = Quaternion.Euler(0, 30 * m_DamageControl.DamageShakeAngle, 0) * Vector3.right;
            // ShakeBase.transform.localRotation = Quaternion.AngleAxis(-30 * m_DamageControl.DamageShakeAmount, axis);

            HandleDeath();

            CheckAlert();
        }

        public virtual void HandleDeath()
        {
            m_DamageControl = GetComponent<DamageControl>();
            //Death
            if (m_DamageControl.Damage <= 0)
            {
                GameObject obj = Instantiate(m_DeathParticlePrefab);
                obj.transform.position = transform.position;
                Destroy(obj, 3);

                SpawnControl.Current.RemoveEnemy(gameObject);
                //GameObject obj1 = Instantiate(m_RagdollPrefab);
                //obj1.transform.position = transform.position;
                //Destroy(obj1, 10);

                DropItem(m_ItemDropCount);

                Destroy(gameObject);
            }
        }

        public virtual void HandleFacePlayer()
        {
            // if (m_FacePlayer)
            // {
            //     Vector3 dir = PlayerChar.m_Current.transform.position - transform.position;
            //     dir.y = 0;

            //     dir.Normalize();
            //     m_RotationBase.rotation = Quaternion.Lerp(m_RotationBase.rotation,Quaternion.LookRotation(dir),10*Time.deltaTime);
            // }
        }


        public virtual void DropItem(int count)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject obj1 = Instantiate(m_ItemPrefabs[0]);
                obj1.transform.position = transform.position + new Vector3(0, .6f, 0);
                obj1.GetComponent<Rigidbody>().linearVelocity = new Vector3(Random.Range(-2, 2), Random.Range(4, 7), Random.Range(-2, 2));
                obj1.GetComponent<Rigidbody>().angularVelocity = new Vector3(Random.Range(-20, 20), Random.Range(-20, 20), Random.Range(-20, 20));
            }
        }

        public virtual void CheckAlert()
        {
            // if (!m_Alerted)
            // {
            //     if (CameraControl.m_Current.m_CameraTopPosition.z > transform.position.z - 5f)
            //     {
            //         StartAlert();
            //     }

            // }
        }
        public virtual void StartAlert()
        {
            m_Alerted = true;

        }

        public void AllowDamage()
        {
            m_CanDamage = true;
        }



        public virtual void EnableEnemy()
        {

        }

        void OnDrawGizmos()
        {

            Gizmos.color = Color.red;
            //Gizmos.DrawLine(transform.position,MoveTargetPosition + new Vector3(0, 0.2f, 0));
            //Gizmos.DrawSphere(MoveTargetPosition, .5f);

        }
    }

}