using UnityEngine;

public class FriedEggController : MonoBehaviour
{
    // Enum to represent the cooking states. It's cleaner than using booleans.
    public enum CookingState { Raw, Cooked, Burnt }
    public CookingState currentState = CookingState.Raw;

    // --- References to the UI Windows ---
    [SerializeField] private GameObject successUI;
    [SerializeField] private GameObject failureUI;

    // These public functions are called by the Pan's script
    public void SetCooked()
    {
        // We only transition from Raw to Cooked.
        if (currentState == CookingState.Raw)
        {
            currentState = CookingState.Cooked;
            Debug.Log("Egg state is now: COOKED");
        }
    }

    public void SetBurnt()
    {
        // We only transition from Cooked to Burnt.
        if (currentState == CookingState.Cooked)
        {
            currentState = CookingState.Burnt;
            Debug.Log("Egg state is now: BURNT");
        }
    }

    // Called when the fried egg collides with something
    private void OnCollisionEnter(Collision collision)
    {
        // Check if we've been placed on the plate
        if (collision.gameObject.CompareTag("Plate"))
        {
            Debug.Log("Fried egg placed on plate. Checking state...");
            
            // We can no longer pick it up.
            // Disable grabbing by destroying the grab interactable component.
            Destroy(GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>());
            // Make the egg non-physical so it rests nicely.
            Destroy(GetComponent<Rigidbody>());

            // --- Show the correct UI based on our state ---
            if (currentState == CookingState.Cooked)
            {
                Debug.Log("Result: SUCCESS!");
                successUI.SetActive(true);
            }
            else // This covers both Raw (if somehow moved) and Burnt
            {
                Debug.Log("Result: FAILURE!");
                failureUI.SetActive(true);
            }
        }
    }
}