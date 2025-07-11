using UnityEngine;

[RequireComponent(typeof(AudioSource))] // Ensures we always have an AudioSource
public class PlacementSoundTrigger : MonoBehaviour
{
    [Tooltip("The audio clip to play when the correct object is placed.")]
    [SerializeField] private AudioClip placementSound;

    [Tooltip("The tag of the object we are waiting for (e.g., 'Grabbable').")]
    [SerializeField] private string targetTag = "Interactable";

    private AudioSource audioSource;
    private bool hasObjectBeenPlaced = false; // Prevents the sound from playing repeatedly

    void Awake()
    {
        // Get the AudioSource component attached to this same GameObject
        audioSource = GetComponent<AudioSource>();
    }

    // This function is called automatically by Unity's physics engine
    // when another collider enters our trigger zone.
    private void OnTriggerEnter(Collider other)
    {
        // First, check if the sound has already been played to avoid repeats.
        // Then, check if the object that entered has the correct tag.
        if (!hasObjectBeenPlaced && other.CompareTag(targetTag))
        {
            Debug.Log(other.name + " has been placed on the target. Playing sound.");

            // Check if we have a sound clip assigned before trying to play it
            if (placementSound != null)
            {
                // Play the sound
                audioSource.PlayOneShot(placementSound);

                // Set the flag to true so it doesn't play again
                hasObjectBeenPlaced = true;
            }
            else
            {
                Debug.LogWarning("PlacementSoundTrigger is missing an AudioClip!", this);
            }
        }
    }

    // (Optional) This function resets the trigger if the object is removed
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            Debug.Log(other.name + " has been removed from the target. Ready for next placement.");
            hasObjectBeenPlaced = false; // Allow the sound to be played again next time
        }
    }
}