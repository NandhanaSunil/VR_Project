//using UnityEngine;

//// This script should be placed on each individual stove button.
//// Each button needs its own collider.
//public class BurnerToggleController : MonoBehaviour, IInteractable
//{
//    [Header("Required Components")]
//    [Tooltip("Drag the Particle System GameObject for the flame here.")]
//    [SerializeField] private ParticleSystem flameEffect;

//    [Header("Optional Components")]
//    [Tooltip("Drag the GameObject with the looping fire sound here.")]
//    [SerializeField] private AudioSource flameSound;

//    // This variable tracks the state of this specific burner.
//    private bool isBurnerOn = false;

//    /// <summary>
//    /// This is a special Unity function that runs once when the object is
//    // first loaded, before the game starts. We use it to ensure everything is off.
//    /// </summary>
//    void Awake()
//    {
//        // Safety check to make sure the flame isn't playing when the game begins.
//        if (flameEffect != null)
//        {
//            // Stop(true) clears any existing particles immediately.
//            flameEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
//        }

//        // Also ensure the sound is stopped.
//        if (flameSound != null)
//        {
//            flameSound.Stop();
//        }
//    }

//    /// <summary>
//    /// This is the public method required by our IInteractable interface.
//    /// The MouseClickManager will call this method when this object is clicked.
//    /// </summary>
//    public void Interact()
//    {
//        // First, do a critical check. If the flameEffect hasn't been assigned
//        // in the Inspector, print an error and stop to prevent further issues.


//        Debug.Log("Interact method called on button: " + this.gameObject.name);
//        if (flameEffect == null)
//        {
//            Debug.LogError("ERROR: The 'flameEffect' has not been assigned on " + this.gameObject.name + "!", this.gameObject);
//            return;
//        }

//        // Toggle the state: if it was on, it becomes off, and vice-versa.
//        isBurnerOn = !isBurnerOn;

//        // Now, apply the new state.
//        if (isBurnerOn)
//        {
//            // --- Turn the burner ON ---
//            flameEffect.Play();

//            // If a sound effect has been assigned, play it.
//            if (flameSound != null)
//            {
//                flameSound.Play();
//            }
//        }
//        else
//        {
//            // --- Turn the burner OFF ---
//            flameEffect.Stop();

//            // If a sound effect has been assigned, stop it.
//            if (flameSound != null)
//            {
//                flameSound.Stop();
//            }
//        }
//    }
//}


using UnityEngine;

public class BurnerToggleController : MonoBehaviour, IInteractable
{
    [Header("Effects")]
    [SerializeField] private ParticleSystem flameEffect;
    [SerializeField] private AudioSource flameAudioSource;

    private bool isBurnerOn = false;

    // This method is required by the IInteractable interface.
    public void Interact()
    {
        Debug.Log("Interact method called on button: " + this.gameObject.name);
        if (flameEffect == null)
        {
            Debug.LogError("FLAME EFFECT IS NULL. Assign it in the Inspector!");
            return;
        }

        isBurnerOn = !isBurnerOn;

        // Add this new log to see the state
        Debug.Log("Burner state is now: " + (isBurnerOn ? "ON" : "OFF"));

        if (isBurnerOn)
        {
            Debug.Log("Trying to PLAY the flame effect...");
            flameEffect.Play();
        }
        else
        {
            Debug.Log("Trying to STOP the flame effect...");
            flameEffect.Stop();
        }
    }
}