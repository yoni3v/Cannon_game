using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CanonShooter
{
    public class GameControl : MonoBehaviour
    {
        public static GameControl m_Main;

        public GameObject m_GameOverEffect;
        private void Awake()
        {
            m_Main = this;
        }
        void Start()
        {
            StartCoroutine(Co_StartGame());
            m_GameOverEffect.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }

        IEnumerator Co_StartGame()
        {
            PlayerCanon.m_Main.m_CanShoot = false;
            AimAtPointer.m_Main.m_AimEnable = false;

            Vector3 pos = new Vector3(2, 8, -4);
            CameraControl.m_Main.transform.position = pos;
            CameraControl.m_Main.transform.forward = -pos;

            yield return new WaitForSeconds(1);
            Vector3 newPos = Quaternion.Euler(70, 0, 0) * (30 * Vector3.forward);
            CameraControl.m_Main.BlendMove(-newPos, Quaternion.LookRotation(newPos, Vector3.up), 3);
            yield return new WaitForSeconds(3);

            UISystem.ShowUI("HUD");
            PlayerCanon.m_Main.m_CanShoot = true;
            CameraControl.m_Main.m_IsAiming = true;
            SpawnControl.Current.StartSpawnLoop();
            AimAtPointer.m_Main.m_AimEnable = true;
        }

        public void HandleGameOver()
        {
            StartCoroutine(Co_EndGame());
        }

        IEnumerator Co_EndGame()
        {
            CameraControl.m_Main.m_IsAiming = false;
            SpawnControl.Current.StopAllEnemies();
            SpawnControl.Current.StopAllCoroutines();
            AimAtPointer.m_Main.m_AimEnable = false;
            yield return new WaitForSeconds(.2f);
            m_GameOverEffect.SetActive(true);
            yield return new WaitForSeconds(1f);
            UISystem.ShowUI("GameOverUI");
        }
    }
}