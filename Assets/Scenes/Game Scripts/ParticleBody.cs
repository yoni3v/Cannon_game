using UnityEngine;

public class ParticleBody : MonoBehaviour
{
    public CursorPositionInWorldSpace target_;

    public bool Grabed = false;

    private void Start()
    {
        target_ = FindAnyObjectByType<CursorPositionInWorldSpace>();
    }

    private void FixedUpdate()
    {
        if (Grabed)
        {
            Vector3 direction = (transform.position - target_.output);
            
            //start damping towards the target

        }
    }
}