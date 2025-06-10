using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CanonShooter
{
    public class ItemCell : MonoBehaviour
    {
        public GameObject m_ParticlePrefab1;
        bool Grabed = false;
        // Start is called before the first frame update
        void Start()
        {
            Grabed = false;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void FixedUpdate()
        {
            if (AimAtPointer.m_Main.m_MagnetActive)
            {
                Vector3 dir = AimAtPointer.m_Main.m_AimPoint - transform.position;
                dir.y = 0;
                if (dir.magnitude <= 10)
                {
                    Grabed = true;
                }

                if (Grabed)
                {
                    Rigidbody body = GetComponent<Rigidbody>();
                    body.linearVelocity += dir;
                    body.linearVelocity -= 0.1f * body.linearVelocity;
                }
            }
            else
            {
                Grabed = false;
            }
        }

        public void Picked()
        {
            GameObject obj = Instantiate(m_ParticlePrefab1);
            obj.transform.position = transform.position;
            Destroy(obj, 3);

            Destroy(gameObject);
        }
    }
}
