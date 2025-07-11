using UnityEngine;

public class StoveTop : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is a pan with our script
        FillableContainer pan = other.GetComponent<FillableContainer>();
        if (pan != null)
        {
            // Tell the pan it's in the right place
            pan.SetReadyToFill(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the object leaving the trigger is our pan
        FillableContainer pan = other.GetComponent<FillableContainer>();
        if (pan != null)
        {
            // Tell the pan it can no longer be filled
            pan.SetReadyToFill(false);
        }
    }
}