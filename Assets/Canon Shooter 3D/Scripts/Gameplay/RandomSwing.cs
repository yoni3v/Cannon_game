using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CanonShooter
{
    public class RandomSwing : MonoBehaviour
    {
        public Vector3 m_Radius = new Vector3(1, 1, 1);
        public Vector3 m_AxisSpeed = new Vector3(.2f, .2f, .2f);
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Vector3 offset = Vector3.zero;
            offset.x = m_Radius.x * Mathf.Cos(m_AxisSpeed.x * Time.time);
            offset.y = m_Radius.y * Mathf.Cos(m_AxisSpeed.y * Time.time);
            offset.z = m_Radius.z * Mathf.Cos(m_AxisSpeed.z * Time.time);

            transform.localPosition = offset;
        }
    }
}