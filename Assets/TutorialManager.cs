using UnityEngine;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    // A more detailed enum to match your new steps
    public enum TutorialStep
    {
        Welcome,                // "Hello"
        PlacePot,               // "Place the pot..."
        PourMilk,               // "Pour some milk..."
        TurnOnFlame,            // "Turn on the stove..."
        WaitForBoil,            // "Let it boil..."
        GoToCoffeeJar,          // "Take the spoon to the coffee jar..."
        CoffeeScooped,          // "Now you have coffee..." -> "Pour it in..."
        GoToSugarJar,           // A new step for sugar
        SugarScooped,           // A new step for when sugar is scooped
        PourInSugar,            // Instruction to pour sugar in
        WaitForPourToCup,       // A state waiting for the final action
        CoffeeReady,            // "Yeyy coffee is ready"
        Complete
    }
    public TutorialStep currentStep;

    [Header("Audio Components")]
    public AudioSource audioSource;
    public AudioClip[] instructionClips;

    [Header("Game Object References")]
    public ParticleSystem stoveFlames;

    private Coroutine activeInstructionCoroutine;
    // NEW: Flags to track if we've added both ingredients
    private bool coffeeIsAdded = false;
    private bool sugarIsAdded = false;

    void Awake()
    {
        if (instance == null) instance = this; else Destroy(gameObject);
    }

    void Start()
    {
        GoToStep(TutorialStep.Welcome);
    }

    void GoToStep(TutorialStep nextStep)
    {
        // Prevent accidentally going back or repeating a step
        if (nextStep <= currentStep && nextStep != TutorialStep.Welcome) return;

        currentStep = nextStep;
        Debug.Log("Advancing to tutorial step: " + nextStep.ToString());

        if (activeInstructionCoroutine != null) StopCoroutine(activeInstructionCoroutine);

        switch (nextStep)
        {
            case TutorialStep.Welcome:
                activeInstructionCoroutine = StartCoroutine(PlayInstructionSequence(new[] { instructionClips[0], instructionClips[1] }));
                break;
            case TutorialStep.PlacePot:
                activeInstructionCoroutine = StartCoroutine(PlayInstructionSequence(new[] { instructionClips[2] }));
                break;
            case TutorialStep.PourMilk:
                activeInstructionCoroutine = StartCoroutine(PlayInstructionSequence(new[] { instructionClips[3] }));
                break;
            case TutorialStep.TurnOnFlame:
                activeInstructionCoroutine = StartCoroutine(PlayInstructionSequence(new[] { instructionClips[4] }));
                break;
            case TutorialStep.WaitForBoil:
                activeInstructionCoroutine = StartCoroutine(PlayInstructionSequence(new[] { instructionClips[5] }));
                break;
            case TutorialStep.GoToCoffeeJar:
                activeInstructionCoroutine = StartCoroutine(PlayInstructionSequence(new[] { instructionClips[6] }));
                break;
            case TutorialStep.CoffeeScooped:
                activeInstructionCoroutine = StartCoroutine(PlayInstructionSequence(new[] { instructionClips[7], instructionClips[8] }));
                break;
            case TutorialStep.GoToSugarJar: // This is triggered after coffee is added
                activeInstructionCoroutine = StartCoroutine(PlayInstructionSequence(new[] { instructionClips[9] })); // A new audio clip: "Now do the same for sugar"
                break;
            case TutorialStep.SugarScooped: // We'll just reuse clip 8: "Pour it into the pot"
                activeInstructionCoroutine = StartCoroutine(PlayInstructionSequence(new[] { instructionClips[8] }));
                break;
            case TutorialStep.WaitForPourToCup: // This step has no audio, just waits for the user
                break;
            case TutorialStep.CoffeeReady:
                activeInstructionCoroutine = StartCoroutine(PlayInstructionSequence(new[] { instructionClips[10] }));
                GoToStep(TutorialStep.Complete); // Mark the tutorial as finished
                break;
        }
    }

    IEnumerator PlayInstructionSequence(AudioClip[] clips)
    {
        foreach (var clip in clips)
        {
            if (clip == null) continue;
            audioSource.PlayOneShot(clip);
            yield return new WaitForSeconds(clip.length + 0.2f);
        }
    }

    void Update()
    {
        if (currentStep == TutorialStep.PourMilk && stoveFlames.isPlaying)
        {
            GoToStep(TutorialStep.TurnOnFlame);
        }
    }

    // --- Public Functions for Other Scripts to Call ---

    public void OnPotPlacedOnStove()
    {
        if (currentStep == TutorialStep.Welcome) GoToStep(TutorialStep.PlacePot);
    }

    public void OnMilkPoured()
    {
        if (currentStep == TutorialStep.PlacePot) GoToStep(TutorialStep.PourMilk);
    }

    public void OnBoilingFinished() // Called when the beep happens
    {
        if (currentStep == TutorialStep.TurnOnFlame) GoToStep(TutorialStep.WaitForBoil);
        // User's audio flow correction: beep happens, *then* instruction to add coffee.
        if (currentStep == TutorialStep.WaitForBoil) GoToStep(TutorialStep.GoToCoffeeJar);
    }

    public void OnPowderScooped(string powderTag)
    {
        if (powderTag == "CoffeeSource" && currentStep == TutorialStep.GoToCoffeeJar)
        {
            GoToStep(TutorialStep.CoffeeScooped);
        }
        else if (powderTag == "SugarSource" && currentStep == TutorialStep.GoToSugarJar)
        {
            GoToStep(TutorialStep.SugarScooped);
        }
    }

    public void OnIngredientAdded(string ingredientName)
    {
        if (ingredientName.ToLower() == "coffee") coffeeIsAdded = true;
        if (ingredientName.ToLower() == "sugar") sugarIsAdded = true;

        // If we just added coffee, and we still need sugar
        if (coffeeIsAdded && !sugarIsAdded && currentStep == TutorialStep.CoffeeScooped)
        {
            GoToStep(TutorialStep.GoToSugarJar);
        }
        // If we just added sugar, and we still need coffee
        else if (sugarIsAdded && !coffeeIsAdded && currentStep == TutorialStep.SugarScooped)
        {
            // This case handles adding sugar first. Let's send them back to the coffee step.
            GoToStep(TutorialStep.GoToCoffeeJar);
        }
        // If both are added, we are ready for the final step!
        else if (coffeeIsAdded && sugarIsAdded)
        {
            GoToStep(TutorialStep.WaitForPourToCup);
        }
    }

    public void OnCupFilled()
    {
        if (currentStep == TutorialStep.WaitForPourToCup) GoToStep(TutorialStep.CoffeeReady);
    }
}