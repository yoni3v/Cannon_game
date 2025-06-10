using UnityEngine;

[CreateAssetMenu(fileName = "Projectile Asset", menuName = "AHEK assets/Projectile", order = 1)]
public class item : ScriptableObject
{
    [Header("Data")]
    public int Projectile_ID;
    public string ProjectileName;

    [Header("Set up")]
    public GameObject Projectile_Object;
    public Sprite logo;

    [Header("Attributes")]
    public float speed = 1;
    public Vector3 rotation_offset;
}