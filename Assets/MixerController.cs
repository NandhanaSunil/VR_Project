using UnityEngine;
using System.Collections;

public class MixerController : MonoBehaviour
{
    public bool isJuiceReady = false;

    // --- NEW ---
    // A private variable to remember if the lid is open or closed
    private bool isLidOpen = true;

    public AudioSource mixerSound;
    public float mixDuration = 4f;

    // --- NEW: The missing function ---
    // This function can be called by your lid script.
    // It takes a boolean (true/false) value as a parameter.
    public void SetLidStatus(bool lidIsOpen)
    {
        isLidOpen = lidIsOpen;
        if (isLidOpen)
        {
            Debug.Log("Mixer lid has been opened.");
        }
        else
        {
            Debug.Log("Mixer lid has been closed.");
        }
    }

    public void AddIngredient(string ingredientTag)
    {
        // --- NEW: Add a check to see if the lid is open ---
        if (!isLidOpen)
        {
            Debug.Log("Cannot add ingredient, the lid is closed!");
            return; // Stop the function here
        }

        if (ingredientTag == "OrangeSlice")
        {
            Debug.Log("An orange was added!");
            StartCoroutine(MixingSequence());
        }
    }

    private IEnumerator MixingSequence()
    {
        // --- NEW: Add a check to see if the lid is closed ---
        if (isLidOpen)
        {
            Debug.Log("Cannot mix, please close the lid first!");
            // Optional: Play an error sound
            yield break; // This stops the coroutine immediately
        }

        Debug.Log("Mixing has started...");
        mixerSound.Play();

        yield return new WaitForSeconds(mixDuration);

        mixerSound.Stop();
        isJuiceReady = true;
        Debug.Log("Mixing complete! Juice is ready.");
    }
}