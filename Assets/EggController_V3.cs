using UnityEngine;

public class EggController_V3 : MonoBehaviour
{
    // A flag to check if we are inside the pan's special cooking area
    private bool isOverPan = false;
    private bool isCracked = false;

    // This function is called by the Pan's trigger script when we enter its zone
    public void SetOverPan(bool status)
    {
        isOverPan = status;
        Debug.Log("Egg is now over pan: " + status);
    }

    // This function is called when a physics collision happens
    private void OnCollisionEnter(Collision collision)
    {
        // Conditions to crack the egg:
        // 1. We must not be already cracked.
        // 2. We must be inside the pan's trigger zone (isOverPan is true).
        // 3. The object that hit us must be the knife.
        if (!isCracked && isOverPan && collision.gameObject.CompareTag("Knife"))
        {
            Debug.Log("Conditions met! Cracking egg over the pan.");
            isCracked = true;

            // Find the knife's AudioSource to play the cracking sound.
            AudioSource knifeSound = collision.gameObject.GetComponent<AudioSource>();
            if (knifeSound != null)
            {
                knifeSound.Play();
            }

            // Tell the pan's trigger zone to create the fried egg and start frying.
            // We use FindObjectOfType for simplicity here.
            PanCookingTrigger panTrigger = FindObjectOfType<PanCookingTrigger>();
            if(panTrigger != null)
            {
                panTrigger.SpawnFriedEgg();
            }

            // Destroy this whole egg.
            Destroy(gameObject);
        }
    }
}