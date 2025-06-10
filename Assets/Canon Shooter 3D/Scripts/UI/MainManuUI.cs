using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace CanonShooter
{
    public class MainManuUI : MonoBehaviour
    {
        bool m_Started = false;
        // Start is called before the first frame update
        void Start()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            m_Started = false;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void BtnStart()
        {
            if (!m_Started)
                StartCoroutine(Co_StartGame());
        }
        public void BtnCredits()
        {

        }

        public void BtnExit()
        {
            Application.Quit();
        }

        IEnumerator Co_StartGame()
        {
            m_Started = true;
            FadeControl.m_Current.StartFadeOut();
            yield return new WaitForSeconds(2);
            SceneManager.LoadScene("Gameplay");
        }

    }
}