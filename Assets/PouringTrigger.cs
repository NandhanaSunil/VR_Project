using UnityEngine;

public class PouringTrigger : MonoBehaviour
{
    private MixerGrinderController grinderController;
    private bool isActivated = false;

    // This is called by the main grinder when the juice is ready
    public void Activate(MixerGrinderController controller)
    {
        grinderController = controller;
        isActivated = true;
    }

    // This is called every frame a trigger stays inside another
    void OnTriggerStay(Collider other)
    {
        // We only do something if the juice is ready and the grinder is tilted
        if (!isActivated || !IsTilted()) return;

        // Try to get a filler script from the object we are touching
        ContainerFiller filler = other.GetComponent<ContainerFiller>();
        if (filler != null)
        {
            // If we found one, tell it to fill itself with juice
            filler.FillWithJuice(Time.deltaTime * 50f); // The number controls fill speed
        }
    }

    private bool IsTilted()
    {
        // Check if the main grinder object (the parent) is tilted past 45 degrees
        return Vector3.Angle(transform.parent.up, Vector3.up) > 45f;
    }
}