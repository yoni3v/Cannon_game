using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
namespace CanonShooter
{
    public class PlayerHitEffect : MonoBehaviour
    {
        public static PlayerHitEffect m_Main;
        public PostProcessVolume m_Volume;
        public Animator m_Anim;

        private void Awake()
        {
            m_Main = this;
        }
        void Start()
        {
            m_Volume.weight = 0;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void StartGlow()
        {
            m_Anim.SetTrigger("glow");
        }
    }
}
