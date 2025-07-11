using UnityEngine;
using System.Collections; // This line is ESSENTIAL for using Coroutines (like WaitForSeconds)

public class PanHitDetector : MonoBehaviour
{
    [Header("Game Object References")]
    [Tooltip("The disabled fried egg model that will appear in the pan.")]
    [SerializeField] private GameObject friedEggModel;

    [Header("Audio Sources")]
    [Tooltip("The AudioSource for the one-shot cracking sound.")]
    [SerializeField] private AudioSource crackingSound;

    [Tooltip("The AudioSource for the looping frying sound.")]
    [SerializeField] private AudioSource fryingSound;
    
    [Tooltip("The AudioSource for the alarm that rings when cooking is done.")]
    [SerializeField] private AudioSource alarmSound;

    [Header("Gameplay Settings")]
    [Tooltip("How many times does the egg need to hit the pan to crack?")]
    [SerializeField] private int hitsToCrack = 2;
    
    [Tooltip("How long the egg cooks before the alarm rings.")]
    [SerializeField] private float cookingTime = 10.0f;
    
    [Tooltip("How long after the alarm the player has before the egg burns.")]
    [SerializeField] private float burnTime = 10.0f;

    // --- Private State Variables ---
    private int hitCount = 0;
    private bool isCracked = false;
    private GameObject eggThatHitMe; // To keep track of which egg is being used

    /// <summary>
    /// This function is called by Unity every time a physics collision occurs.
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        // First, check if the egg is already cracked in this pan. If so, do nothing.
        if (isCracked)
        {
            return;
        }

        // Check if the object that hit us is tagged "Egg".
        if (collision.gameObject.CompareTag("Egg"))
        {
            // If this is the first time an egg has hit us, store a reference to it.
            if (eggThatHitMe == null)
            {
                eggThatHitMe = collision.gameObject;
            }

            // Make sure we are only counting hits from the *same* egg.
            // This prevents the player from tapping with two different eggs.
            if (collision.gameObject == eggThatHitMe)
            {
                hitCount++;
                Debug.Log("Pan hit by egg. Hit count: " + hitCount);

                // Play the cracking sound on every hit.
                if (crackingSound != null)
                {
                    crackingSound.Play();
                }

                // Check if we have reached the required number of hits.
                if (hitCount >= hitsToCrack)
                {
                    PerformCrack();
                }
            }
        }
    }

    /// <summary>
    /// This function contains all the actions for when the egg finally cracks.
    /// It activates the model and starts the cooking timer sequence.
    /// </summary>
    private void PerformCrack()
    {
        Debug.Log("Egg has cracked! Starting cooking timer.");
        isCracked = true;

        // Make sure the fried egg model is assigned before trying to use it.
        if (friedEggModel != null)
        {
            friedEggModel.SetActive(true);
        }
        else
        {
            Debug.LogError("Fried Egg Model is not assigned in the Inspector!");
            return; // Stop here if the egg model is missing.
        }

        // Start the looping frying sound.
        if (fryingSound != null)
        {
            fryingSound.Play();
        }

        // Destroy the original whole egg.
        if (eggThatHitMe != null)
        {
            Destroy(eggThatHitMe);
        }

        // Start the cooking process as a coroutine.
        StartCoroutine(CookingSequence());
    }

    /// <summary>
    /// This Coroutine handles the timed events for cooking and burning the egg.
    /// </summary>
    private IEnumerator CookingSequence()
    {
        // --- STEP 1: Wait for the cooking time ---
        Debug.Log("Cooking for " + cookingTime + " seconds.");
        yield return new WaitForSeconds(cookingTime);

        // --- STEP 2: Cooking is done, ring the alarm ---
        Debug.Log("Cooking finished. Ringing alarm!");
        if (alarmSound != null)
        {
            alarmSound.Play();
        }

        // The egg is now perfectly cooked. We tell its script.
        FriedEggController eggController = friedEggModel.GetComponent<FriedEggController>();
        if (eggController != null)
        {
            eggController.SetCooked();
        }
        else
        {
            Debug.LogWarning("Fried Egg Model is missing the FriedEggController script.");
        }

        // --- STEP 3: Wait for the burn time ---
        Debug.Log("Player has " + burnTime + " seconds to remove the egg.");
        yield return new WaitForSeconds(burnTime);

        // --- STEP 4: Time's up! Burn the egg ---
        // We only burn the egg if it's still in the "Cooked" state (meaning the player hasn't moved it).
        if (eggController != null && eggController.currentState == FriedEggController.CookingState.Cooked)
        {
            Debug.Log("Egg is now burnt!");
            eggController.SetBurnt();
            // Optional: You could add a function call here to change the egg's material to a darker "burnt" one.
            // eggController.ChangeToBurntMaterial();
        }
    }
}