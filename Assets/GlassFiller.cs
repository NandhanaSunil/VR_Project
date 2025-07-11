using UnityEngine;

public class GlassFiller : MonoBehaviour
{
    // A slot for our liquid visual
    public GameObject liquidVisual;

    void Start()
    {
        // Start with the liquid hidden
        liquidVisual.SetActive(false);
    }

    // This runs when the glass's trigger touches another collider
    void OnTriggerEnter(Collider other)
    {
        // Check if the object we touched has the MixerController script
        MixerController mixer = other.GetComponent<MixerController>();

        // If it's a mixer AND its juice is ready...
        if (mixer != null && mixer.isJuiceReady)
        {
            // Make our liquid appear!
            liquidVisual.SetActive(true);
            Debug.Log("Glass has been filled with juice!");
        }
    }
}