//using UnityEngine;

//public class BowlFiller : MonoBehaviour
//{
//    [Header("Liquid Settings")]
//    public Transform liquidSurface; // Drag your "WaterLiquid" here
//    public float maxFillAmount = 150f; // Bowls might hold more

//    private float currentFillAmount = 0f;
//    private Vector3 fullLiquidScale;

//    void Start()
//    {
//        if (liquidSurface != null)
//        {
//            fullLiquidScale = liquidSurface.localScale;
//            liquidSurface.localScale = new Vector3(fullLiquidScale.x, 0f, fullLiquidScale.z);
//        }
//    }

//    void OnParticleCollision(GameObject other)
//    {
//        if (currentFillAmount < maxFillAmount)
//        {
//            Debug.Log("collided filling");
//            currentFillAmount += 0.2f; // Adjust fill speed if needed
//            UpdateLiquidVisuals();
//        }
//    }

//    private void UpdateLiquidVisuals()
//    {
//        if (liquidSurface == null) return;

//        float fillRatio = currentFillAmount / maxFillAmount;
//        fillRatio = Mathf.Min(fillRatio, 1.0f);

//        float newHeight = fullLiquidScale.y * fillRatio;
//        liquidSurface.localScale = new Vector3(fullLiquidScale.x, newHeight, fullLiquidScale.z);
//    }
//}


using UnityEngine;

public class BowlFiller : MonoBehaviour
{
    [Header("Liquid Settings")]
    public Transform liquidSurface;
    public float maxFillAmount = 150f;

    private float currentFillAmount = 0f;
    private Vector3 fullLiquidScale;

    // --- NEW: A public property to check if there is any liquid ---
    public bool IsFilled { get { return currentFillAmount > 0.1f; } }

    void Start()
    {
        if (liquidSurface != null)
        {
            fullLiquidScale = liquidSurface.localScale;
            liquidSurface.localScale = new Vector3(fullLiquidScale.x, 0f, fullLiquidScale.z);
        }
    }

    void OnParticleCollision(GameObject other)
    {
        if (currentFillAmount < maxFillAmount)
        {
            currentFillAmount += 0.2f;
            UpdateLiquidVisuals();
        }
    }

    // --- NEW: A public function to drain the liquid ---
    public void DrainLiquid(float drainAmount)
    {
        // Decrease the fill amount, but don't go below zero
        currentFillAmount = Mathf.Max(0f, currentFillAmount - drainAmount);
        UpdateLiquidVisuals();
    }

    private void UpdateLiquidVisuals()
    {
        if (liquidSurface == null) return;

        float fillRatio = currentFillAmount / maxFillAmount;
        fillRatio = Mathf.Min(fillRatio, 1.0f);

        float newHeight = fullLiquidScale.y * fillRatio;
        liquidSurface.localScale = new Vector3(fullLiquidScale.x, newHeight, fullLiquidScale.z);
    }
}