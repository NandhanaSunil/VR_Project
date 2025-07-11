//using UnityEngine;
//using UnityEngine.XR.Interaction.Toolkit; // We need this for the socket

//public class StoveController : MonoBehaviour
//{
//    [Header("Stove Components")]
//    // Drag your stove's flame particle system here
//    public ParticleSystem flameParticles;
//    // Drag the XRSocketInteractor from your stove here
//    public XRSocketInteractor stoveSocket;
//    // Drag the AudioSource with the BOILING sound here
//    public AudioSource boilingSound;

//    private PotFiller currentPot = null;

//    void Update()
//    {
//        // Check all the conditions for boiling every frame
//        CheckBoilingStatus();
//    }

//    private void CheckBoilingStatus()
//    {
//        // Condition 1: Is the flame particle system on?
//        bool flameIsOn = flameParticles != null && flameParticles.isPlaying;

//        // Condition 2: Is there a pot in the socket?
//        bool potIsOnStove = stoveSocket.hasSelection;

//        // Condition 3: If there's a pot, is it filled?
//        bool potIsFilled = false;
//        if (potIsOnStove)
//        {
//            // --- THIS IS THE CORRECTED LINE ---
//            // Get the object currently in the socket using the new property
//            IXRSelectInteractable potObject = stoveSocket.firstInteractableSelected;

//            // Try to get the PotFiller script from it
//            if (potObject != null) // Add a null check for safety
//            {
//                currentPot = potObject.transform.GetComponent<PotFiller>();
//                if (currentPot != null)
//                {
//                    // Use our new "IsFilled" property to check
//                    potIsFilled = currentPot.IsFilled;
//                }
//            }
//        }

//        // Final Check: Are ALL conditions true?
//        if (flameIsOn && potIsOnStove && potIsFilled)
//        {
//            // If they are all true and the sound isn't playing yet, play it.
//            if (!boilingSound.isPlaying)
//            {
//                boilingSound.Play();
//            }
//        }
//        else
//        {
//            // If any condition is false and the sound is currently playing, stop it.
//            if (boilingSound.isPlaying)
//            {
//                boilingSound.Stop();
//            }
//        }
//    }
//}


using UnityEngine;


public class StoveController : MonoBehaviour
{
    [Header("Stove Components")]
    public ParticleSystem flameParticles;
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor stoveSocket;

    [Header("Audio Sources")]
    public AudioSource boilingSound;
    // NEW: Drag the AudioSource with the BEEP sound here
    public AudioSource beepSound;

    [Header("Timing Settings")]
    // NEW: Public variable to control the delay
    public float timeUntilBeep = 15f;

    // --- Private state variables ---
    private PotFiller currentPot = null;
    // NEW: A timer to track boiling duration
    private float boilingTimer = 0f;
    // NEW: A flag to ensure the beep only plays once per session
    private bool hasBeeped = false;


    void Update()
    {
        CheckBoilingStatus();
    }

    private void CheckBoilingStatus()
    {
        bool flameIsOn = flameParticles != null && flameParticles.isPlaying;
        bool potIsOnStove = stoveSocket.hasSelection;
        bool potIsFilled = false;

        if (potIsOnStove)
        {
            UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable potObject = stoveSocket.firstInteractableSelected;
            if (potObject != null)
            {
                currentPot = potObject.transform.GetComponent<PotFiller>();
                if (currentPot != null)
                {
                    potIsFilled = currentPot.IsFilled;
                }
            }
        }

        // Check if all conditions for boiling are true
        if (flameIsOn && potIsOnStove && potIsFilled)
        {
            // If boiling should start but isn't already...
            if (!boilingSound.isPlaying)
            {
                boilingSound.Play();
                // Reset our timer and flag for a new session
                boilingTimer = 0f;
                hasBeeped = false;
            }

            // --- NEW TIMER LOGIC ---
            // If the beep hasn't happened yet...
            if (!hasBeeped)
            {
                // ...increment our timer by the time since the last frame.
                boilingTimer += Time.deltaTime;

                // Check if the timer has reached our target time.
                if (boilingTimer >= timeUntilBeep)
                {
                    // Play the beep sound!
                    if (beepSound != null)
                    {
                        beepSound.Play();
                        TutorialManager.instance.OnBoilingFinished();
                    }
                    // Set the flag to true so we don't beep again this session.
                    hasBeeped = true;
                }
            }
        }
        else // If any condition for boiling is false...
        {
            // ...stop the boiling sound if it's currently playing.
            if (boilingSound.isPlaying)
            {
                boilingSound.Stop();
                // We can also reset the state here, just in case.
                boilingTimer = 0f;
                hasBeeped = false;
            }
        }
    }
}