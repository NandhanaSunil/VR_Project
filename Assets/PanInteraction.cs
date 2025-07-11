using UnityEngine;

public class PanInteraction : MonoBehaviour
{
    [Tooltip("The audio source for the frying sound.")]
    [SerializeField] private AudioSource fryingAudioSource;

    void Awake()
    {
        // Ensure the frying sound doesn't play at the start
        if (fryingAudioSource != null)
        {
            fryingAudioSource.playOnAwake = false;
            fryingAudioSource.loop = true; // Frying sound should loop
        }
    }

    // This is a public method that the egg will call when it cracks
    public void StartFrying(GameObject bullsEye)
    {
        Debug.Log("Frying has started!");
        if (fryingAudioSource != null)
        {
            fryingAudioSource.Play();
        }

        // Make the bull's eye a child of the pan so it moves with it
        bullsEye.transform.SetParent(this.transform);
    }
}