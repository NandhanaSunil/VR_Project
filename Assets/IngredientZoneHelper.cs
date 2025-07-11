using UnityEngine;

public class IngredientZoneHelper : MonoBehaviour
{
    // A slot to hold the Mixer's main script
    public MixerController mainMixerController;

    // This function runs automatically when the trigger is touched
    void OnTriggerEnter(Collider other)
    {
        // Tell the main controller what just entered, using its tag
        mainMixerController.AddIngredient(other.tag);

        // Destroy the object that entered to make it disappear
        Destroy(other.gameObject);
    }
}