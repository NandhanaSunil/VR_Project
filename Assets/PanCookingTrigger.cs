using UnityEngine;

public class PanCookingTrigger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject friedEggModel; // The hidden fried egg
    [SerializeField] private AudioSource fryingSound;   // The pan's frying audio source

    // Called when an object with a collider/rigidbody ENTERS our trigger zone
    private void OnTriggerEnter(Collider other)
    {
        // Check if it's the egg
        if (other.CompareTag("Egg"))
        {
            // Get the egg's script and tell it that it's now over the pan
            other.GetComponent<EggController_V3>()?.SetOverPan(true);
        }
    }

    // Called when an object LEAVES our trigger zone
    private void OnTriggerExit(Collider other)
    {
        // Check if it's the egg
        if (other.CompareTag("Egg"))
        {
            // Get the egg's script and tell it that it's no longer over the pan
            other.GetComponent<EggController_V3>()?.SetOverPan(false);
        }
    }

    // This public function is called by the egg right before it's destroyed
    public void SpawnFriedEgg()
    {
        // Make the fried egg visible
        if (friedEggModel != null)
        {
            friedEggModel.SetActive(true);
        }

        // Start the frying sound
        if (fryingSound != null)
        {
            fryingSound.Play();
        }
    }
}