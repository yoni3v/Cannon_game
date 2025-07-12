using UnityEngine;

public class ParticleBody : MonoBehaviour
{
    CursorPositionInWorldSpace target_;
    Vector3 CurrentVelocity;

    public Vector3 Offset;
    public float Smooth_time;
    public float RandomOffsetTicks = 0.3f;
    public float Randomness_Scalar = 1.5f;
    public float Lifetime = 15;
    public bool UseDeltaTime = true;
    float elapsettime = 0;

    private void Start()
    {
        elapsettime = RandomOffsetTicks;
        target_ = FindAnyObjectByType<CursorPositionInWorldSpace>();
        Destroy(gameObject, Lifetime);
    }

    private void FixedUpdate()
    {
        if (elapsettime > 0)
        {
            elapsettime -= Time.deltaTime;

            if (elapsettime <= 0)
            {
                Offset = new Vector3(Random.Range(-0.5f, 0.5f) * Randomness_Scalar, 0, Random.Range(-0.5f, 0.5f) * Randomness_Scalar);
                elapsettime = RandomOffsetTicks;
            }
        }

        //start damping towards the target
        transform.position = Vector3.SmoothDamp(transform.position, target_.output + Offset, ref CurrentVelocity, Smooth_time * (UseDeltaTime ? Time.deltaTime : 0));
    }
}