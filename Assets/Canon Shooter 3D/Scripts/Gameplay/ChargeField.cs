using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CanonShooter
{
    public class ChargeField : MonoBehaviour
    {
        public int m_Charge = 0;
        public int MaxCharge = 20;
        public Transform m_Bar;

        public GameObject m_SpawnPrefab;
        public GameObject m_SpawnParticle1;
        public GameObject m_SpawnParticle2;
        public GameObject m_ChargeParticlePrefab;

        public bool m_CanObsorb = true;
        // Start is called before the first frame update
        void Start()
        {
            m_CanObsorb = true;
        }

        // Update is called once per frame
        void Update()
        {
            float a = (float)m_Charge / (float)MaxCharge;
            m_Bar.localScale = new Vector3(a, 1, 1);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (m_CanObsorb)
            {
                if (other.gameObject.tag == "Cell")
                {
                    m_Charge++;
                    m_Charge = Mathf.Clamp(m_Charge, 0, MaxCharge);

                    ItemCell cell = other.gameObject.GetComponent<ItemCell>();
                    cell.Picked();

                    GameObject obj1 = Instantiate(m_ChargeParticlePrefab);
                    obj1.transform.position = transform.position;
                    Destroy(obj1, 3);

                    if (m_Charge == MaxCharge)
                    {
                        m_Charge = 0;
                        StartCoroutine(Co_CreateObject());

                    }
                }
            }
        }

        IEnumerator Co_CreateObject()
        {
            m_CanObsorb = false;

            GameObject obj1 = Instantiate(m_SpawnParticle1);
            obj1.transform.position = transform.position;
            Destroy(obj1, 2.2f);

            yield return new WaitForSeconds(2.4f);

            obj1 = Instantiate(m_SpawnParticle2);
            obj1.transform.position = transform.position;
            Destroy(obj1, 5);

            GameObject obj = Instantiate(m_SpawnPrefab);
            obj.transform.position = transform.position + new Vector3(0, 1, 0);
            Rigidbody body = obj.GetComponent<Rigidbody>();
            body.linearVelocity = new Vector3(0, 6, 0);
            body.angularVelocity = new Vector3(Random.Range(-20, 20), Random.Range(-20, 20), Random.Range(-20, 20));

            yield return new WaitForSeconds(1f);
            m_CanObsorb = true;
        }
    }
}