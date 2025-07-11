using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Pourer : MonoBehaviour
{
    [Header("Pouring Properties")]
    [SerializeField] private ParticleSystem milkStreamParticleSystem; // Assign the milk particle system
    [SerializeField] private Transform spoutOrigin; // Assign the empty 'Spout' object
    [SerializeField][Range(0, 180)] private float pourThresholdAngle = 70f;
    [SerializeField] private float pourCheckDistance = 1.0f;

    private bool isPouring = false;
    private XRGrabInteractable grabInteractable;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void Update()
    {
        // We only want to check for pouring if the object is being held
        if (grabInteractable != null && grabInteractable.isSelected)
        {
            // Check the angle of the bottle to see if it's tilted enough
            float bottleAngle = Vector3.Angle(Vector3.up, transform.up);

            if (bottleAngle > pourThresholdAngle)
            {
                StartPour();
            }
            else
            {
                StopPour();
            }
        }
        else
        {
            // Ensure we stop pouring if the object is released
            StopPour();
        }
    }

    private void StartPour()
    {
        if (isPouring) return; // Already pouring, do nothing

        isPouring = true;
        milkStreamParticleSystem.Play();

        // Start a coroutine or use Update to continuously check for a container
        // For simplicity, we'll put the raycast check in Update while pouring.
    }

    private void StopPour()
    {
        if (!isPouring) return; // Not pouring, do nothing

        isPouring = false;
        milkStreamParticleSystem.Stop();
    }

    // We can use FixedUpdate for physics-based raycasting
    void FixedUpdate()
    {
        if (isPouring)
        {
            // Raycast from the spout to see what we are pouring into
            if (Physics.Raycast(spoutOrigin.position, spoutOrigin.forward, out RaycastHit hit, pourCheckDistance))
            {
                // Check if the object we hit can be filled
                FillableContainer container = hit.collider.GetComponent<FillableContainer>();
                if (container != null)
                {
                    // If it is, call its Fill method
                    container.Fill();
                }
            }
        }
    }
}