using UnityEngine;

public class MixerLidTrigger : MonoBehaviour
{
    // We need a reference to the main controller to tell it the lid's status.
    [SerializeField] private MixerController mixerController;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered is the lid.
        if (other.CompareTag("MixerLid"))
        {
            // Tell the main controller that the lid is ON.
            mixerController.SetLidStatus(true);
            
            // Optional "Snap" feature:
            // You can make the lid snap into place perfectly.
            Rigidbody lidRb = other.GetComponent<Rigidbody>();
            if (lidRb != null)
            {
                lidRb.isKinematic = true; // Stop it from moving
                other.transform.position = transform.position; // Center it on the trigger
                other.transform.rotation = transform.rotation; // Align its rotation
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the object that left is the lid.
        if (other.CompareTag("MixerLid"))
        {
            // Tell the main controller that the lid is OFF.
            mixerController.SetLidStatus(false);
            
            // Re-enable physics on the lid if it was snapped.
            Rigidbody lidRb = other.GetComponent<Rigidbody>();
            if (lidRb != null)
            {
                lidRb.isKinematic = false;
            }
        }
    }
}