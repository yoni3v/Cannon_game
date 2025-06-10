using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CanonShooter
{
    public class MagnetObject : MonoBehaviour
    {
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
                if (dir.magnitude <= 6)
                {
                    Grabed = true;
                }

                if (Grabed)
                {
                    Rigidbody body = GetComponent<Rigidbody>();
                    body.linearVelocity += .4f * dir;
                    body.linearVelocity -= 0.1f * body.linearVelocity;
                }
            }
            else
            {
                Grabed = false;
            }
        }
    }
}
