// LiquidContainer.cs (Scaling Version)
using UnityEngine;

public class LiquidContainer : MonoBehaviour
{
    [Header("Container Settings")]
    [Tooltip("The maximum amount of liquid this container can hold.")]
    public float maxVolume = 100f;

    [Tooltip("The current amount of liquid in the container.")]
    [SerializeField] private float currentVolume = 0f;

    [Header("Visuals")]
    [Tooltip("Drag the child object representing the water surface here.")]
    [SerializeField] private Transform waterLevelVisual;

    // --- CHANGED SECTION START ---

    [Tooltip("The local Y scale of the water when the bowl is EMPTY.")]
    [SerializeField] private float emptyLevelScaleY = 0.01f; // A very small, flat value

    [Tooltip("The local Y scale of the water when the bowl is FULL.")]
    [SerializeField] private float fullLevelScaleY = 1.0f; // The full height of the visualizer

    // --- CHANGED SECTION END ---

    public bool IsFull => currentVolume >= maxVolume;

    void Start()
    {
        UpdateWaterVisual();
    }

    public void AddLiquid(float amount)
    {
        if (IsFull) return;
        currentVolume = Mathf.Min(currentVolume + amount, maxVolume);
        UpdateWaterVisual();
    }
    //
    private void UpdateWaterVisual()
    {
        if (waterLevelVisual == null) return;
        Debug.Log("Filling!!!!");

        float fillPercent = currentVolume / maxVolume;

        // --- CHANGED SECTION START ---

        // Use Lerp to find the correct Y scale between empty and full
        float targetScaleY = Mathf.Lerp(emptyLevelScaleY, fullLevelScaleY, fillPercent);

        // Apply the new scale. We keep the X and Z scale the same.
        waterLevelVisual.localScale = new Vector3(
            waterLevelVisual.localScale.x,
            targetScaleY,
            waterLevelVisual.localScale.z
        );

        // --- CHANGED SECTION END ---
    }
}