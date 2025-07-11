using UnityEngine;

public class ContainerFiller : MonoBehaviour
{
    [Header("Liquid Visuals")]
    public Transform liquidSurface; // The cylinder object that scales up
    public Color defaultLiquidColor = Color.white; // Default for water/milk
    public Color juiceColor = new Color(1.0f, 0.5f, 0.0f); // Orange juice color

    [Header("Fill Settings")]
    public float maxFillAmount = 100f; // How much liquid it can hold
    public float fillSpeedMultiplier = 1.0f; // Adjust fill speed for this container

    // --- Private Variables ---
    private float currentFillAmount = 0f;
    private Vector3 fullLiquidScale;
    private Renderer liquidRenderer;
    private Color currentLiquidColor;

    void Start()
    {
        if (liquidSurface != null)
        {
            // Get the renderer so we can change its color
            liquidRenderer = liquidSurface.GetComponent<Renderer>();

            // Set the starting color
            currentLiquidColor = defaultLiquidColor;
            if (liquidRenderer != null)
            {
                liquidRenderer.material.color = currentLiquidColor;
            }

            // Remember the full size and then squash it flat
            fullLiquidScale = liquidSurface.localScale;
            liquidSurface.localScale = new Vector3(fullLiquidScale.x, 0f, fullLiquidScale.z);
        }
    }

    // --- METHOD 1: For Particle Collisions (Milk, Water) ---
    void OnParticleCollision(GameObject other)
    {
        // Change color based on the particle system's name (a simple way to check)
        if (other.name.Contains("Milk"))
        {
            currentLiquidColor = defaultLiquidColor; // Assuming milk is the default
        }
        else if (other.name.Contains("Water"))
        {
            currentLiquidColor = defaultLiquidColor; // Or make a new clear color
        }

        // Fill with a standard amount per particle hit
        Fill(0.2f * fillSpeedMultiplier);
    }

    // --- METHOD 2: For "Magic Pour" (Grinder's Juice) ---
    public void FillWithJuice(float amount)
    {
        // When this is called, we know it must be juice
        currentLiquidColor = juiceColor;

        // Fill by the amount passed in from the PouringTrigger
        Fill(amount);
    }

    // --- Central Fill Logic ---
    private void Fill(float amount)
    {
        if (currentFillAmount < maxFillAmount)
        {
            // Increase the fill amount, ensuring it doesn't go over the max
            currentFillAmount = Mathf.Min(maxFillAmount, currentFillAmount + amount);
            UpdateLiquidVisuals();
        }
    }

    // --- Updates the Cylinder's Visuals ---
    private void UpdateLiquidVisuals()
    {
        if (liquidSurface == null) return;

        // Set the color first
        if (liquidRenderer != null)
        {
            liquidRenderer.material.color = currentLiquidColor;
        }

        // Calculate the fill ratio (0.0 to 1.0)
        float fillRatio = currentFillAmount / maxFillAmount;

        // Calculate the new height and apply it
        float newHeight = fullLiquidScale.y * fillRatio;
        liquidSurface.localScale = new Vector3(fullLiquidScale.x, newHeight, fullLiquidScale.z);
    }
}