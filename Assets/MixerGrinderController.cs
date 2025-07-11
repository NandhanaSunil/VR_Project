using UnityEngine;
using System.Collections;

public class MixerGrinderController : MonoBehaviour
{
    public bool IsJuiceReady { get; private set; } = false;

    [Header("Components & Settings")]
    public AudioSource mixerSound;
    public float mixDuration = 5f;
    public PouringTrigger pouringTrigger;

    // --- Private state tracking ---
    private int orangeCount = 0;
    private bool waterAdded = false;
    private bool isMixing = false;

    // --- NEW: Function to be called by the Spoon ---
    public void AddIngredient(string ingredientName)
    {
        if (ingredientName.ToLower() == "orange")
        {
            orangeCount++;
            Debug.Log("Orange slice added via spoon! Total oranges: " + orangeCount);
        }
        else
        {
            Debug.LogWarning("Tried to add invalid ingredient to grinder: " + ingredientName);
        }
    }

    // Called by water particles hitting the grinder's trigger zone
    public void WaterParticleEntered()
    {
        if (!waterAdded)
        {
            waterAdded = true;
            Debug.Log("Water has been added!");
        }
    }

    // Called by the knob interaction
    public void StartMixing()
    {
        if (orangeCount > 0 && waterAdded && !isMixing)
        {
            StartCoroutine(MixingSequence());
        }
        else
        {
            Debug.Log("Cannot mix! Missing ingredients.");
        }
    }

    private IEnumerator MixingSequence()
    {
        isMixing = true;
        Debug.Log("Mixing started!");
        if (mixerSound != null) mixerSound.Play();

        yield return new WaitForSeconds(mixDuration);

        if (mixerSound != null) mixerSound.Stop();

        IsJuiceReady = true;
        if (pouringTrigger != null)
        {
            pouringTrigger.Activate(this);
        }

        Debug.Log("Mixing complete! Juice is ready to be poured.");
    }
}