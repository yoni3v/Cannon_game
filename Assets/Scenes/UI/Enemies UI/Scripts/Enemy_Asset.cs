using UnityEngine;

[CreateAssetMenu(menuName = "AHEK assets/Enemy asset")]
public class Enemy_Asset : ScriptableObject
{
    public string Name;
    public Sprite Picture;
    public GameObject Prefab;
    public bool unlocked = true;
}