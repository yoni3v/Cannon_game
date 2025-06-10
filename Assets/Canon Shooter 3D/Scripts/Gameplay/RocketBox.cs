using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CanonShooter
{
    public class RocketBox : MonoBehaviour
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
            Vector3 dis = transform.position - PlayerCanon.m_Main.transform.position;
            if (dis.magnitude <= 3)
            {
                GameObject obj = Instantiate(m_ParticlePrefab);
                obj.transform.position = PlayerCanon.m_Main.transform.position + new Vector3(0, 1, 0);
                Destroy(obj, 3);

                PlayerCanon.m_Main.GiveRocket();
                Destroy(gameObject);
            }
        }


    }
}
