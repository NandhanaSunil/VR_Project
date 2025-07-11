//using UnityEngine;

//public class BottlePour : MonoBehaviour
//{
//    // Drag your Particle System object here in the Inspector
//    public ParticleSystem milkStreamParticles;

//    // NEW: Drag the Audio Source component from your bottle here
//    public AudioSource pouringSound;

//    // This is the angle the bottle needs to be tilted to start pouring
//    public float pourThreshold = 75f;

//    private bool isPouring = false;

//    void Update()
//    {
//        // Vector3.up is a line pointing straight up (0, 1, 0)
//        // transform.up is the direction the top of our bottle is pointing
//        // Vector3.Angle gives us the angle between these two vectors.
//        float tiltAngle = Vector3.Angle(transform.up, Vector3.up);

//        // If the bottle is tilted more than our threshold...
//        if (tiltAngle > pourThreshold)
//        {
//            // ...start pouring.
//            StartPouring();
//        }
//        else
//        {
//            // ...otherwise, stop pouring.
//            StopPouring();
//        }
//    }

//    private void StartPouring()
//    {
//        // We only want to call these functions once, not every frame.
//        if (!isPouring)
//        {
//            isPouring = true;
//            milkStreamParticles.Play();

//            // NEW: Play the sound. Because "Loop" is checked, it will continue playing.
//            if (pouringSound != null)
//            {
//                pouringSound.Play();
//            }
//        }
//    }

//    private void StopPouring()
//    {
//        // We only want to call these functions once.
//        if (isPouring)
//        {
//            isPouring = false;
//            milkStreamParticles.Stop();

//            // NEW: Stop the sound.
//            if (pouringSound != null)
//            {
//                pouringSound.Stop();
//            }
//        }
//    }
//}

using UnityEngine;

public class BottlePour : MonoBehaviour
{
    // --- Existing Variables ---
    public ParticleSystem milkStreamParticles;
    public AudioSource pouringSound;

    // The angle the bottle needs to be tilted to start pouring
    public float pourThreshold = 75f;

    // --- NEW: Variables for realistic pouring ---
    // The minimum speed when just starting to pour (tilted slightly)
    public float minPourSpeed = 0.5f;
    // The maximum speed when the bottle is fully horizontal
    public float maxPourSpeed = 2.5f;
    // The angle at which the max speed is reached (e.g., 90 degrees for horizontal)
    public float maxSpeedAngle = 90f;


    private bool isPouring = false;
    private ParticleSystem.MainModule psMain; // To efficiently access particle system properties

    void Start()
    {
        // Get the 'main' module of the particle system to control it later
        psMain = milkStreamParticles.main;
    }

    void Update()
    {
        // Calculate the current tilt angle of the bottle
        float tiltAngle = Vector3.Angle(transform.up, Vector3.up);

        if (tiltAngle > pourThreshold)
        {
            // If pouring, update the stream properties
            UpdatePour(tiltAngle);
            StartPouring();
        }
        else
        {
            // Otherwise, stop pouring
            StopPouring();
        }
    }

    private void StartPouring()
    {
        if (!isPouring)
        {
            isPouring = true;
            milkStreamParticles.Play();
            if (pouringSound != null) pouringSound.Play();
        }
    }

    private void StopPouring()
    {
        if (isPouring)
        {
            isPouring = false;
            milkStreamParticles.Stop();
            if (pouringSound != null) pouringSound.Stop();
        }
    }

    // --- NEW: This is the core logic for the realistic pour ---
    private void UpdatePour(float currentAngle)
    {
        // Mathf.InverseLerp calculates a 0-1 value representing how far 'currentAngle' is
        // between our pourThreshold and the maxSpeedAngle.
        // For example, if pourThreshold=75 and maxSpeedAngle=90, and our current angle is 82.5,
        // this will return 0.5 (halfway).
        float pourRatio = Mathf.InverseLerp(pourThreshold, maxSpeedAngle, currentAngle);

        // Mathf.Lerp then uses that 0-1 value to find the corresponding speed
        // between our min and max speeds.
        float calculatedSpeed = Mathf.Lerp(minPourSpeed, maxPourSpeed, pourRatio);

        // Apply the calculated speed to the particle system's startSpeed.
        psMain.startSpeed = calculatedSpeed;
    }
}