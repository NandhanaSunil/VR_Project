using UnityEngine;

public class PotFiller : MonoBehaviour
{
    // Drag the "LiquidSurface" cylinder here
    public Transform liquidSurface;

    // How much liquid is currently in the pot
    private float currentFillAmount = 0f;

    // The maximum amount the pot can hold (you can adjust this)
    public float maxFillAmount = 100f;

    // The maximum Y-scale of your liquid surface when the pot is full
    // Adjust this value in the inspector until it looks right for your pot.
    public float maxLiquidHeight = 0.1f;

    // A flag to see if we have coffee in the pot
    public bool hasCoffee { get; private set; } = false;

    // The color of the liquid when it's just milk
    public Color milkColor = Color.white;
    // The color when coffee is added
    public Color coffeeColor = new Color(0.36f, 0.25f, 0.2f); // A nice brown

    private Renderer liquidRenderer;

    private bool hasNotifiedMilkPoured = false;

    // Add this new variable at the top of the script
    public bool hasSugar { get; private set; } = false;

    // Find your AddIngredient function and expand it
    public void AddIngredient(string ingredientName)
    {
        string ingredient = ingredientName.ToLower();

        if (ingredient == "coffee")
        {
            hasCoffee = true;
            Debug.Log("Coffee added to pot!");
            UpdateLiquidVisuals();
        }
        else if (ingredient == "sugar")
        {
            // We don't have a visual for sugar dissolving, but we track it
            hasSugar = true;
            Debug.Log("Sugar added to pot!");
        }

        // --- NEW ---
        // Notify the TutorialManager what was added.
        TutorialManager.instance.OnIngredientAdded(ingredient);
    }

    public bool IsFilled { get { return currentFillAmount > 0.1f; } }

    void Start()
    {
        // Get the renderer component to change its color later
        if (liquidSurface != null)
        {
            liquidRenderer = liquidSurface.GetComponent<Renderer>();
            UpdateLiquidVisuals();
        }
    }

    // This function is called automatically when a particle from a system
    // with "Send Collision Messages" enabled hits this object's collider.
    void OnParticleCollision(GameObject other)
    {
        // We can add a check here to see if the particle is milk
        // For now, any particle will do.
        Debug.Log("Particles hit the pot, filling!!!!!");
        if (currentFillAmount < maxFillAmount)
        {
            // Increase the fill amount slightly for each collision
            currentFillAmount += 0.1f;
            UpdateLiquidVisuals();

            if (!hasNotifiedMilkPoured && !hasCoffee)
            {
                TutorialManager.instance.OnMilkPoured();
                hasNotifiedMilkPoured = true; // Set flag so we don't call it again
            }
        }
    }

    // A public function we can call from the spoon script
    //public void AddIngredient(string ingredientName)
    //{
    //    if (ingredientName.ToLower() == "coffee")
    //    {
    //        hasCoffee = true;
    //        Debug.Log("Coffee added to pot!");
    //        UpdateLiquidVisuals(); // Update color
    //    }
    //    else if (ingredientName.ToLower() == "sugar")
    //    {
    //        Debug.Log("Sugar added to pot!");
    //        // You could add another flag for sugar if you want
    //    }
    //}

    private void UpdateLiquidVisuals()
    {
        if (liquidSurface == null) return;

        // Calculate the fill percentage (a value from 0.0 to 1.0)
        float fillRatio = currentFillAmount / maxFillAmount;

        // Set the Y-scale of the liquid based on the fill ratio
        liquidSurface.localScale = new Vector3(
            liquidSurface.localScale.x,
            maxLiquidHeight * fillRatio,
            liquidSurface.localScale.z
        );

        // Change the color if coffee has been added and there's liquid
        if (hasCoffee && currentFillAmount > 0)
        {
            liquidRenderer.material.color = coffeeColor;
        }
        else
        {
            liquidRenderer.material.color = milkColor;
        }
    }
}