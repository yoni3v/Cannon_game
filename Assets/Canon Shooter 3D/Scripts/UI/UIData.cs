using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CanonShooter
{
    [CreateAssetMenu(fileName = "UIData", menuName = "CustomObjects/UIData", order = 1)]
    public class UIData : ScriptableObject
    {
        public GameObject[] m_UIPrefabs;
        public GameObject[] m_UIElementsPrefabs;
        public Dictionary<string, GameObject> m_UIPrefabList;


        [Space]
        public AnimationCurve m_AnimInCurve_1;
        public AnimationCurve m_AnimInCurve_2;
    }
}