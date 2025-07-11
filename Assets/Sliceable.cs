using UnityEngine;

public class Sliceable : MonoBehaviour
{
    [Header("Result Object")]
    [Tooltip("The GameObject in the scene that becomes visible after this object is cut (e.g., the parent of the two orange halves).")]
    [SerializeField] private GameObject slicedVersionObject;

    [Header("Audio")]
    [Tooltip("The sound that plays when the object is cut.")]
    [SerializeField] private AudioClip cutSound;

    private bool isCut = false;

    // This function is called by Unity whenever a physics collision begins.
    private void OnCollisionEnter(Collision collision)
    {
        // If this object has already been cut, do nothing to prevent it from running multiple times.
        if (isCut)
        {
            return;
        }

        // Check if the object that hit us is tagged as "Knife".
        if (collision.gameObject.CompareTag("Knife"))
        {
            Debug.Log(gameObject.name + " was sliced by the knife!");

            // Mark as cut so this logic can't run again.
            isCut = true;

            // --- Perform the Slice ---

            // 1. Play the cutting sound at the location of the cut.
            // We use PlayClipAtPoint because this object is about to be destroyed.
            if (cutSound != null)
            {
                AudioSource.PlayClipAtPoint(cutSound, transform.position);
            }

            // 2. Activate the new "sliced" version that is already in the scene.
            if (slicedVersionObject != null)
            {
                slicedVersionObject.SetActive(true);
            }
            else
            {
                Debug.LogError("Sliced Version Object is not assigned in the Inspector!");
            }

            // 3. Destroy this original, whole object.
            Destroy(gameObject);
        }
    }
}