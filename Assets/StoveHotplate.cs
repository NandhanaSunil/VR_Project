using UnityEngine;

public class StoveHotplate : MonoBehaviour
{
    // Drag the AudioSource component from the stove into this slot in the Inspector
    [SerializeField] private AudioSource thudSound;

    // This function is automatically called by Unity when another collider enters this trigger
    private void OnTriggerEnter(Collider other)
    {
        // We check if the object that entered has the "Pan" tag.
        // This prevents the sound from playing if the player's hand or something else touches it.
        if (other.CompareTag("Pan"))
        {
            Debug.Log("Pan has entered the stove trigger! Playing sound.");

            // If the sound source is assigned, play the sound.
            if (thudSound != null)
            {
                thudSound.Play();
            }
        }
    }
}