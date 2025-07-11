using UnityEngine;

public class BowlPourController : MonoBehaviour
{
    [Header("Components")]
    public ParticleSystem waterStreamParticles; // The particles to play

    [Header("Parameters")]
    public float pourThreshold = 45f; // Bowls pour at a shallower angle
    public float drainRate = 20f; // How fast the water drains while pouring

    private bool isPouring = false;
    // A reference to the bowl's own filler script
    private BowlFiller bowlFiller;

    void Start()
    {
        // Get the other script on this same GameObject
        bowlFiller = GetComponent<BowlFiller>();
    }

    void Update()
    {
        // Check if the bowl has liquid before trying to pour
        bool canPour = bowlFiller != null && bowlFiller.IsFilled;

        float tiltAngle = Vector3.Angle(transform.up, Vector3.up);

        // We can only pour if we are tilted AND we have liquid
        if (canPour && tiltAngle > pourThreshold)
        {
            StartPouring();
        }
        else
        {
            StopPouring();
        }

        // If we are currently pouring, drain the liquid
        if (isPouring)
        {
            bowlFiller.DrainLiquid(drainRate * Time.deltaTime);
        }
    }

    private void StartPouring()
    {
        if (!isPouring)
        {
            isPouring = true;
            waterStreamParticles.Play();
        }
    }

    private void StopPouring()
    {
        if (isPouring)
        {
            isPouring = false;
            waterStreamParticles.Stop();
        }
    }
}