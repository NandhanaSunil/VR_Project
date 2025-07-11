using UnityEngine;

public class FillableContainer : MonoBehaviour
{
    [Header("Fill Settings")]
    [SerializeField] private float maxFillAmount = 1.0f;
    [SerializeField] private float fillSpeed = 0.2f; // Liters per second

    [Header("Visuals")]
    [SerializeField] private Transform milkLevelVisual; // Assign the MilkLevelVisual object here
    [SerializeField] private Vector3 maxScale; // The target localScale of the visual when full

    private float currentFillAmount = 0f;
    private bool isReadyToFill = false;
    private Vector3 initialScale;
    private Renderer visualRenderer;

    void Start()
    {
        if (milkLevelVisual != null)
        {
            visualRenderer = milkLevelVisual.GetComponent<Renderer>();
            initialScale = milkLevelVisual.localScale;
            // Start with the visual disabled
            if (visualRenderer != null) visualRenderer.enabled = false;
        }
    }

    // This method will be called by the pouring object
    public void Fill()
    {
        // Only fill if it's placed on the stove and not already full
        if (!isReadyToFill || currentFillAmount >= maxFillAmount)
        {
            return;
        }

        // Enable the visual if it's the first drop
        if (visualRenderer != null && !visualRenderer.enabled)
        {
            visualRenderer.enabled = true;
        }

        // Increase the fill amount
        currentFillAmount += fillSpeed * Time.deltaTime;
        currentFillAmount = Mathf.Clamp(currentFillAmount, 0, maxFillAmount);

        // Update the visual scale based on the fill percentage
        float fillPercent = currentFillAmount / maxFillAmount;
        if (milkLevelVisual != null)
        {
            milkLevelVisual.localScale = Vector3.Lerp(initialScale, maxScale, fillPercent);
        }

        // Optional: Add sound effects or other logic here
        // Debug.Log($"Pan filled to {fillPercent * 100}%");
    }

    // This will be called by the stove's trigger
    public void SetReadyToFill(bool isReady)
    {
        isReadyToFill = isReady;
        Debug.Log("Pan is ready to be filled: " + isReady);
    }
}