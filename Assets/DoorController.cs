using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour, IInteractable
{
    // A boolean to check if the door is open or closed.
    private bool isDoorOpen = false;

    // The target rotation for the door when it's open.
    // You can set this in the Inspector. 90 degrees is a good start.
    [SerializeField] private float openAngle = 90.0f;

    // The rotation of the door when it's closed (usually 0).
    [SerializeField] private float closeAngle = 0.0f;

    // How fast the door opens/closes.
    [SerializeField] private float smoothSpeed = 2.0f;

    // The target rotation we want to reach.
    private Quaternion targetRotation;

    void Start()
    {
        // Set the initial target rotation to the closed state.
        targetRotation = Quaternion.Euler(0, closeAngle, 0);
    }

    void Update()
    {
        // Smoothly rotate the door towards the target rotation.
        // Quaternion.Lerp is used for smooth interpolation between two rotations.
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, smoothSpeed * Time.deltaTime);
    }

    // This public method will be called by another script when the door is clicked.
    public void Interact()
    {
        // Toggle the state of the door.
        isDoorOpen = !isDoorOpen;

        // Set the target rotation based on the new state.
        if (isDoorOpen)
        {
            targetRotation = Quaternion.Euler(0, openAngle, 0);
        }
        else
        {
            targetRotation = Quaternion.Euler(0, closeAngle, 0);
        }
    }
}