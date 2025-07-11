//using UnityEngine;
//using System.Collections;

//public class PourController : MonoBehaviour
//{
//    [Header("Object References")]
//    [Tooltip("The child object that has the visual model to be tilted.")]
//    public Transform modelToTilt; // We will drag our 'MilkBottle_Model' here

//    [Tooltip("The particle system that simulates the milk pour.")]
//    public ParticleSystem pourParticles;

//    [Header("Pouring Settings")]
//    [Tooltip("The angle the model will tilt to when pouring sideways.")]
//    public float pourAngle = 90f; // 90 degrees is a full sideways pour

//    [Tooltip("How quickly the model tilts into the pour position.")]
//    public float tiltSpeed = 7f;

//    private Coroutine activePourCoroutine;

//    void Start()
//    {
//        if (pourParticles != null)
//        {
//            pourParticles.Stop();
//        }
//    }

//    // This is called when the parent object's collider enters the trigger
//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag("PouringZone"))
//        {
//            if (activePourCoroutine != null)
//            {
//                StopCoroutine(activePourCoroutine);
//            }
//            activePourCoroutine = StartCoroutine(TiltAndPour());
//        }
//    }

//    // This is called when the parent object's collider exits the trigger
//    private void OnTriggerExit(Collider other)
//    {
//        if (other.CompareTag("PouringZone"))
//        {
//            if (activePourCoroutine != null)
//            {
//                StopCoroutine(activePourCoroutine);
//            }
//            activePourCoroutine = StartCoroutine(StraightenUp());
//        }
//    }

//    private IEnumerator TiltAndPour()
//    {
//        if (pourParticles != null)
//        {
//            pourParticles.Play();
//        }

//        // *** THIS IS THE KEY TO A RIGHT TILT ***
//        // We create a LOCAL rotation around the Z-axis (roll).
//        // A negative angle on the Z-axis creates a clockwise, RIGHTWARD roll.
//        Quaternion targetRotation = Quaternion.Euler(0, 0, -pourAngle);

//        // Smoothly rotate the CHILD MODEL to the target local rotation
//        while (Quaternion.Angle(modelToTilt.localRotation, targetRotation) > 1f)
//        {
//            modelToTilt.localRotation = Quaternion.Slerp(modelToTilt.localRotation, targetRotation, Time.deltaTime * tiltSpeed);
//            yield return null;
//        }
//    }

//    private IEnumerator StraightenUp()
//    {
//        if (pourParticles != null)
//        {
//            pourParticles.Stop();
//        }

//        // The target is the default, upright local rotation ("no rotation")
//        Quaternion targetRotation = Quaternion.identity;

//        // Smoothly rotate the CHILD MODEL back to its original local rotation
//        while (Quaternion.Angle(modelToTilt.localRotation, targetRotation) > 1f)
//        {
//            modelToTilt.localRotation = Quaternion.Slerp(modelToTilt.localRotation, targetRotation, Time.deltaTime * tiltSpeed);
//            yield return null;
//        }

//        // Snap to the final rotation to ensure it's perfect
//        modelToTilt.localRotation = Quaternion.identity;
//    }
//}


using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// This script controls the pouring behavior of a particle system,
// directing it towards a specific target.
public class PourController : MonoBehaviour
{
    [Header("Pouring Components")]
    [Tooltip("The Particle System that creates the liquid stream effect.")]
    public ParticleSystem milkParticleSystem;

    [Tooltip("The empty GameObject that marks where the liquid should land.")]
    public Transform pourTarget;

    [Header("Pouring Parameters")]
    [Tooltip("How fast the liquid particles will travel towards the target.")]
    public float pourSpeed = 3.0f;

    // We need to cache the module to modify it at runtime
    private ParticleSystem.VelocityOverLifetimeModule velocityModule;
    private bool isPouring = false;

    void Awake()
    {
        // Get the Velocity over Lifetime module from the particle system
        if (milkParticleSystem != null)
        {
            velocityModule = milkParticleSystem.velocityOverLifetime;
        }
    }

    void Update()
    {
        if (isPouring && milkParticleSystem != null && pourTarget != null)
        {
            // Calculate the standard direction
            Vector3 directionToTarget = (pourTarget.position - milkParticleSystem.transform.position).normalized;

            // Invert the direction by multiplying by -1
            Vector3 finalVelocity = directionToTarget * -1;

            // Apply this inverted direction to the particle's velocity
            // Make sure you multiply by pourSpeed!
            velocityModule.x = finalVelocity.x * pourSpeed;
            velocityModule.y = finalVelocity.y * pourSpeed;
            velocityModule.z = finalVelocity.z * pourSpeed;
        }
    }

    // This public method will be called by the XR Socket Interactor's "Select Entered" event
    public void StartPouring()
    {
        isPouring = true;

        // Enable the velocity module and start the particle emission
        if (milkParticleSystem != null)
        {
            velocityModule.enabled = true;
            milkParticleSystem.Play();
        }
    }

    // This public method will be called by the XR Socket Interactor's "Select Exited" event
    public void StopPouring()
    {
        isPouring = false;

        // Stop the particle emission and disable the velocity module
        if (milkParticleSystem != null)
        {
            milkParticleSystem.Stop();
            velocityModule.enabled = false;
        }
    }
}