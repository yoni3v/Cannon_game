using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CanonShooter
{
    public class ExplosionBox : MonoBehaviour
    {
        [SerializeField]
        protected GameObject m_ParticlePrefab;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            DamageControl damage = GetComponent<DamageControl>();
            if (damage.IsDead)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, 8);
                foreach (Collider c in colliders)
                {
                    if (c.gameObject == gameObject)
                        continue;

                    DamageControl otherDamage = c.gameObject.GetComponent<DamageControl>();
                    if (otherDamage != null)
                    {
                        otherDamage.ApplyDamage(5, Vector3.forward, 1);
                    }
                }

                GameObject obj = Instantiate(m_ParticlePrefab);
                obj.transform.position = transform.position;
                Destroy(obj, 3);

                CameraControl.m_Main.StartShake(.5f, .3f);
                Destroy(gameObject);
            }
        }
    }
}