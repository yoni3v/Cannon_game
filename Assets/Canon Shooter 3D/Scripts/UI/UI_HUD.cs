using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace CanonShooter
{
    public class UI_HUD : MonoBehaviour
    {
        public Image m_DamageOverlay;
        public Text[] m_PlayerTexts_1;
        public Text m_GemCountText;
        public Text m_GunNameText;
        public Text m_KillCountText;
        public Text m_EnemyWaveText;
        public Image m_CursorImage;
        public Image m_AimTargetImage;
        public RectTransform m_MainCanvas;

        public Text m_ReloadText;
        public Image m_WeaponPowerTime;
        public Image m_PlayerHealth;

        [Space]
        public Image m_BossHealthBase;
        public Image m_BossHealth;

        [Space]
        public Image m_PowerBase;
        public Image m_PowerBar;
        public Text m_PowerNameText;
        public Text m_PowerAmountText;

        public string[] m_WeaponNames = new string[4] { "PISTOL", "SHOTGUN", "MACHINGUN", "PLASMA GUN" };

        public static UI_HUD m_Main;

        void Awake()
        {
            m_Main = this;
        }
        // Start is called before the first frame update
        void Start()
        {
            Cursor.visible = false;
            m_ReloadText.gameObject.SetActive(false);
            //m_BossHealthBase.gameObject.SetActive(false);
        }

        public void ReticleEff()
        {
            StartCoroutine(Co_ReticleEff());
        }
        IEnumerator Co_ReticleEff()
        {
            m_CursorImage.transform.localScale = 1.3f * Vector3.one;
            yield return new WaitForSeconds(.05f);
            m_CursorImage.transform.localScale = Vector3.one;
        }

        // Update is called once per frame
        void Update()
        {
            //m_GemCountText.text = PlayerControl.MainPlayerController.m_GemCount.ToString();

            Vector3 v = Input.mousePosition;

            v.x = v.x / (float)Screen.width;
            v.y = v.y / (float)Screen.height;

            v.x = m_MainCanvas.sizeDelta.x * v.x;
            v.y = m_MainCanvas.sizeDelta.y * v.y;

            m_CursorImage.rectTransform.anchoredPosition = Helper.ToVector2(v);


            m_KillCountText.text = SpawnControl.Current.TotalKillCount.ToString();
            m_EnemyWaveText.text = (SpawnControl.Current.SpawnWaveNum + 1).ToString();

            v = Vector3.zero;

            Color c = Color.white;
            c.a = .6f + 0.4f * Mathf.Sin(10 * Time.time);
            m_ReloadText.color = c;
            m_ReloadText.transform.localScale = (1 + .2f * (float)Math.Sin(10 * Time.time)) * Vector3.one;
            //if (PlayerChar.m_Current.m_LockedTarget!=null)
            //{
            //    m_AimTargetImage.gameObject.SetActive(true);
            //    m_AimTargetImage.color = Color.red;
            //    v = CameraControl.m_Current.m_MyCamera.WorldToScreenPoint(PlayerChar.m_Current.m_LockedTarget.transform.position);
            //}
            //else
            //{
            //if (PlayerChar.m_Current.m_TempTarget != null)
            //{
            //    m_AimTargetImage.gameObject.SetActive(true);
            //    Color c = Color.white;
            //    c.a = .4f;
            //    m_AimTargetImage.color = c;
            //    v = CameraControl.m_Current.m_MyCamera.WorldToScreenPoint(PlayerChar.m_Current.m_TempTarget.m_TargetCenter.position);
            //}
            //else
            //{
            //    m_AimTargetImage.gameObject.SetActive(false);
            //}
            //}


            //v.x = v.x / Screen.width;
            //v.y = v.y / Screen.height;

            //v.x = m_MainCanvas.sizeDelta.x * v.x - 0.5f * m_MainCanvas.sizeDelta.x;
            //v.y = m_MainCanvas.sizeDelta.y * v.y - 0.5f * m_MainCanvas.sizeDelta.y;

            //m_AimTargetImage.rectTransform.localPosition = v;



            //m_WeaponPowerTime.fillAmount = PlayerChar.m_Current.m_WpnPowerTime / 16f;

            //DamageControl damage = PlayerChar.m_Current.GetComponent<DamageControl>();
            //m_PlayerHealth.fillAmount = damage.Damage / damage.MaxDamage;


            //m_GunNameText.text = m_WeaponNames[PlayerChar.m_Current.m_WeaponNum];

            //if (GameControl.m_Current.m_LevelBoss != null)
            //{
            //    damage = GameControl.m_Current.m_LevelBoss.GetComponent<DamageControl>();
            //    m_BossHealth.fillAmount = damage.Damage / damage.MaxDamage;
            //}

            //PlayerPowers p = PlayerChar.m_Current.GetComponent<PlayerPowers>();
            //if (p.m_HavePower)
            //{
            //    m_PowerBase.gameObject.SetActive(true);
            //    switch(p.m_PowerNum)
            //    {
            //        case 0:
            //            m_PowerNameText.text = "Grenade";
            //            m_PowerAmountText.gameObject.SetActive(true);
            //            m_PowerAmountText.text = p.m_AmmoCount.ToString();
            //            m_PowerBar.gameObject.SetActive(false);
            //            break;
            //        case 1:
            //            m_PowerNameText.text = "Bomb";
            //            m_PowerAmountText.gameObject.SetActive(true);
            //            m_PowerAmountText.text = p.m_AmmoCount.ToString();
            //            m_PowerBar.gameObject.SetActive(false);
            //            break;
            //    }
            //}
            //else
            //{
            //    m_PowerBase.gameObject.SetActive(false);
            //}
            //for (int i = 0; i < 1; i++)
            //{
            //    float amount1 = GameControl.m_Current.m_Players[i].m_DamageControl.Damage / GameControl.m_Current.m_Players[i].m_DamageControl.MaxDamage;
            //    Color c = Color.red;
            //    c.a = 0.4f * (1 - amount1);
            //    m_DamageOverlays[i].color = c;
            //}


            //m_PlayerTexts_1[0].text = "Pistol : " + GameControl.m_Current.m_Players[0].m_MyWeaponControl.m_AmmoCount[0].ToString();
            //m_PlayerTexts_1[1].text = "Pistol : " + GameControl.m_Current.m_Players[0].m_MyWeaponControl.m_AmmoCount[1].ToString();
            //m_PlayerTexts_1[2].text = "Pistol : " + GameControl.m_Current.m_Players[0].m_MyWeaponControl.m_AmmoCount[2].ToString();
            //m_PlayerTexts_1[3].text = "Pistol : " + GameControl.m_Current.m_Players[0].m_MyWeaponControl.m_AmmoCount[3].ToString();
            //m_PlayerTexts_1[4].text = "Health : " + ((int)(GameControl.m_Current.m_Players[0].m_DamageControl.Damage*10)).ToString();
            //m_PlayerTexts_1[5].text = "Resource : " + GameControl.m_Current.m_Players[0].m_ResourceAmount.ToString();
            //m_AmmoText.text ="Ammo : "+ Player.m_Current.m_WeaponAmmo1.ToString();


            //m_CarRepairBar.fillAmount = GameControl.m_Current.m_CarRepairAmount / 1000f;
        }

        public void ShowBossHealth()
        {
            m_BossHealthBase.gameObject.SetActive(true);
        }
        public void HideBossHealth()
        {
            m_BossHealthBase.gameObject.SetActive(false);
        }

    }
}
