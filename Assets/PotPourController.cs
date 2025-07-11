using UnityEngine;

public class PotPourController : MonoBehaviour
{
    [Header("Pouring Components")]
    public ParticleSystem coffeeStreamParticles;
    // NEW: Drag the AudioSource with the coffee pouring sound here
    public AudioSource pouringSound;

    [Header("Pouring Parameters")]
    public float pourThreshold = 60f;
    public float minPourSpeed = 1f;
    public float maxPourSpeed = 3f;
    public float maxSpeedAngle = 90f;

    private PotFiller potFiller;
    private bool isPouring = false;
    private ParticleSystem.MainModule psMain;

    void Start()
    {
        potFiller = GetComponent<PotFiller>();
        psMain = coffeeStreamParticles.main;
    }

    void Update()
    {
        bool canPour = potFiller != null && potFiller.IsFilled;
        float tiltAngle = Vector3.Angle(transform.up, Vector3.up);

        if (canPour && tiltAngle > pourThreshold)
        {
            UpdatePour(tiltAngle);
            StartPouring();
        }
        else
        {
            StopPouring();
        }
    }

    private void StartPouring()
    {
        if (!isPouring)
        {
            isPouring = true;
            coffeeStreamParticles.Play();

            // NEW: Play the pouring sound
            if (pouringSound != null)
            {
                pouringSound.Play();
            }
        }
    }

    private void StopPouring()
    {
        if (isPouring)
        {
            isPouring = false;
            coffeeStreamParticles.Stop();

            // NEW: Stop the pouring sound
            if (pouringSound != null)
            {
                pouringSound.Stop();
            }
        }
    }

    private void UpdatePour(float currentAngle)
    {
        float pourRatio = Mathf.InverseLerp(pourThreshold, maxSpeedAngle, currentAngle);
        float calculatedSpeed = Mathf.Lerp(minPourSpeed, maxPourSpeed, pourRatio);
        psMain.startSpeed = calculatedSpeed;
    }
}