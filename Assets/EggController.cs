using UnityEngine;

public class EggController : MonoBehaviour
{
    [Header("Assets")]
    [Tooltip("The Bull's Eye prefab to spawn.")]
    [SerializeField] private GameObject bullsEyePrefab;
    [Tooltip("The sound of the egg cracking.")]
    [SerializeField] private AudioClip crackingSound;

    private bool isCracked = false;

    // This is called when this object's collider touches another collider
    private void OnCollisionEnter(Collision collision) // << The 'collision' variable is defined here
    {
        // Prevent it from cracking multiple times
        if (isCracked) return;

        // We check if the object we hit has the "InteractableSurface" TAG (like the pan)
        if (collision.gameObject.CompareTag("InteractableSurface"))
        {
            Debug.Log("Egg hit the pan! Cracking...");
            
            // --- THIS IS THE FIXED LINE ---
            // Now we pass BOTH the surface object AND the collision data
            CrackTheEgg(collision.gameObject, collision); 
        }
    }

    // --- THIS IS THE OTHER FIXED LINE ---
    // The method now accepts the Collision data it needs
    private void CrackTheEgg(GameObject surfaceObject, Collision collision) 
    {
        isCracked = true;

        // 1. Play the cracking sound at the point of collision
        AudioSource.PlayClipAtPoint(crackingSound, transform.position);

        // 2. Spawn the Bull's Eye prefab
        // This line needs the 'collision' variable, which is why we passed it in.
        ContactPoint contact = collision.contacts[0]; 
        Vector3 spawnPosition = contact.point + contact.normal * 0.01f;
        // Get the rotation of the surface we hit (the pan)
        Quaternion surfaceRotation = surfaceObject.transform.rotation;
        GameObject bullsEyeInstance = Instantiate(bullsEyePrefab, spawnPosition, surfaceRotation);

        // 3. Tell the pan to start frying
        PanInteraction pan = surfaceObject.GetComponent<PanInteraction>();
        if (pan != null)
        {
            pan.StartFrying(bullsEyeInstance);
        }

        // 4. Destroy the original uncracked egg
        Destroy(gameObject);
    }
}