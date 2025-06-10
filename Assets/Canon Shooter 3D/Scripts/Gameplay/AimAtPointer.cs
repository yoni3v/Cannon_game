using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CanonShooter
{
    public class AimAtPointer : MonoBehaviour
    {
        public Transform m_BaseTransform;
        public Camera m_Camera;

        [HideInInspector]
        public Vector3 m_AimPoint;
        [HideInInspector]
        public bool m_MagnetActive = false;
        [HideInInspector]
        public bool m_AimEnable = false;

        public static AimAtPointer m_Main;

        public GameObject m_MagnetParticlePrefab;
        [HideInInspector]
        public GameObject m_MagnetParticle;
        private void Awake()
        {
            m_Main = this;
        }
        // Start is called before the first frame update
        void Start()
        {
            m_AimEnable = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (m_AimEnable)
            {
                Vector3 mousePos = Input.mousePosition;
                if (Input.touchCount>0)
                {
                    mousePos = Input.touches[0].position;
                }
                Ray ray = m_Camera.ScreenPointToRay(mousePos);
                Plane plane = new Plane(Vector3.up, new Vector3(0, 1, 0));
                float dis = 0;
                plane.Raycast(ray, out dis);
                Vector3 pos = ray.origin + dis * ray.direction;
                pos.y = 0;
                m_AimPoint = pos;

                Vector3 dir = pos - m_BaseTransform.position;
                dir.y = 0;

                m_BaseTransform.rotation = Quaternion.Lerp(m_BaseTransform.rotation, Quaternion.LookRotation(dir, Vector3.up), 10 * Time.deltaTime);

                if (Input.GetMouseButton(0) || Input.touchCount>0)
                {
                    if (!m_MagnetActive)
                    {
                        m_MagnetParticle = Instantiate(m_MagnetParticlePrefab);
                    }
                    m_MagnetParticle.transform.position = m_AimPoint + new Vector3(0, 1, 0);
                    m_MagnetActive = true;
                }
                else
                {
                    if (m_MagnetActive)
                    {
                        Destroy(m_MagnetParticle);
                    }
                    m_MagnetActive = false;
                }
            }
        }
    }
}