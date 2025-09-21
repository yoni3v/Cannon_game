using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "AHEK assets/Particle")]
public class Particles : ScriptableObject
{
    public string Name;
    public Sprite Picture;
    public ParticleSystem Particle;
    public bool unlocked = true;
}