using UnityEngine;

public class FaucetController : MonoBehaviour, IInteractable
{
    [Header("Effects")]
    [SerializeField] private ParticleSystem waterParticleSystem;
    [SerializeField] private AudioSource waterAudioSource;

    private bool isWaterOn = false;

    // This method is required by the IInteractable interface.
    public void Interact()
    {
        Debug.Log("interact calleddd !!!");
        isWaterOn = !isWaterOn; // Toggle the state

        if (isWaterOn)
        {
            // Turn the water ON
            waterParticleSystem.Play();
            waterAudioSource.Play();
        }
        else
        {
            // Turn the water OFF
            // Using Stop() is better than Pause() because it lets existing
            // particles finish their life gracefully.
            waterParticleSystem.Stop();
            waterAudioSource.Stop();
        }
    }
}