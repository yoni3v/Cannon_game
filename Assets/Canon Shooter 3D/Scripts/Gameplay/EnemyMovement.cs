using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CanonShooter
{
    public class EnemyMovement : MonoBehaviour
    {
        public float m_MoveSpeed = 2;
        public bool m_IsMoving = true;
        // Start is called before the first frame update
        void Start()
        {
            m_IsMoving = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (m_IsMoving)
            {
                Vector3 targetPos = PlayerCanon.m_Main.transform.position;
                Vector3 dir = targetPos - transform.position;
                dir.y = 0;
                dir.Normalize();
                transform.position += m_MoveSpeed * Time.deltaTime * dir;
                transform.forward = dir;
            }
        }
    }
}
