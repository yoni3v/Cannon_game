using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CanonShooter
{
    public class GameOverUI : MonoBehaviour
    {
        bool m_Started = false;
        // Start is called before the first frame update
        void Start()
        {
            m_Started = false;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void BtnRestart()
        {
            if (!m_Started)
                StartCoroutine(Co_StartGame());
        }

        public void BtnExit()
        {
            SceneManager.LoadScene("MainMenu");
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