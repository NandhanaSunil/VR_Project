using UnityEngine;

public class CupFiller : MonoBehaviour
{
    [Header("Liquid Settings")]
    public Transform liquidSurface; // Drag your "CoffeeLiquid" here
    public float maxFillAmount = 50f; // Cups hold less than pots

    private float currentFillAmount = 0f;
    private Vector3 fullLiquidScale;


    // Add this new variable at the top of the CupFiller script
    private bool hasNotifiedCupFilled = false;

    // Find your OnParticleCollision method and add this check
    void OnParticleCollision(GameObject other)
    {
        if (currentFillAmount < maxFillAmount)
        {
            // ... (your existing fill logic) ...

            currentFillAmount += 0.2f; // Fill the cup a bit faster per particle
            UpdateLiquidVisuals();

            // --- NEW ---
            if (!hasNotifiedCupFilled)
            {
                TutorialManager.instance.OnCupFilled();
                hasNotifiedCupFilled = true; // Set flag
            }
        }
    }

    void Start()
    {
        if (liquidSurface != null)
        {
            // Remember the full size
            fullLiquidScale = liquidSurface.localScale;
            // Start squashed flat
            liquidSurface.localScale = new Vector3(fullLiquidScale.x, 0f, fullLiquidScale.z);
        }
    }

    //void OnParticleCollision(GameObject other)
    //{
    //    if (currentFillAmount < maxFillAmount)
    //    {
    //        currentFillAmount += 0.2f; // Fill the cup a bit faster per particle
    //        UpdateLiquidVisuals();
    //    }
    //}



    private void UpdateLiquidVisuals()
    {
        if (liquidSurface == null) return;

        float fillRatio = currentFillAmount / maxFillAmount;
        fillRatio = Mathf.Min(fillRatio, 1.0f);

        float newHeight = fullLiquidScale.y * fillRatio;
        liquidSurface.localScale = new Vector3(fullLiquidScale.x, newHeight, fullLiquidScale.z);
    }
}