using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CanonShooter
{
    public class RocketExplosion : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 5);
            foreach (Collider c in colliders)
            {
                if (c.gameObject == gameObject)
                    continue;

                DamageControl otherDamage = c.gameObject.GetComponent<DamageControl>();
                if (otherDamage != null)
                {
                    otherDamage.ApplyDamage(3, Vector3.forward, 1);
                }
            }

            CameraControl.m_Main.StartShake(.6f, .5f);
            //Destroy(gameObject);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}