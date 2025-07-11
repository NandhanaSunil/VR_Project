using UnityEngine;

public class SpoonController : MonoBehaviour
{
    // Drag the "PowderVisual" child object (your sphere) here
    public GameObject powderVisual;
    //public static TutorialManager instance;

    // You can set these colors in the Inspector!
    public Color coffeeColor = new Color(0.4f, 0.2f, 0.1f); // A nice coffee brown
    public Color sugarColor = Color.white;

    private bool hasPowder = false;
    private string powderType = ""; // To know if we have coffee or sugar
    private Renderer powderRenderer; // We'll store the renderer of the powder sphere

    void Start()
    {
        // Make sure the visual is off at the start
        if (powderVisual != null)
        {
            powderVisual.SetActive(false);
            // Get the Renderer component from our powder visual object
            powderRenderer = powderVisual.GetComponent<Renderer>();

            // Error check in case you forget to add a renderer
            if (powderRenderer == null)
            {
                Debug.LogError("The PowderVisual object is missing a Renderer component!");
            }
        }
    }

    // This is called when the spoon's collider ENTERS another trigger collider
    void OnTriggerEnter(Collider other)
    {
        // --- SCOOPING LOGIC ---
        // If we don't have powder yet and we touch a powder source...
        if (!hasPowder)
        {
            // Check if we hit the coffee
            if (other.CompareTag("CoffeeSource"))
            {
                ScoopPowder("CoffeeSource", coffeeColor);
            }
            // Check if we hit the sugar
            else if (other.CompareTag("SugarSource"))
            {
                ScoopPowder("SugarSource", sugarColor);
            }
        }

        // --- DEPOSITING LOGIC ---
        // If we HAVE powder and we enter the pot...
        if (hasPowder && other.CompareTag("Pot"))
        {
            // Get the PotFiller script from the pot object
            PotFiller pot = other.GetComponent<PotFiller>();

            // If the script exists, call the AddIngredient function
            if (pot != null)
            {
                string ingredient = powderType.Replace("Source", "");
                pot.AddIngredient(ingredient);
            }

            // Reset the spoon
            ResetSpoon();
        }
    }

    // A helper function to handle the scooping action
    private void ScoopPowder(string type, Color color)
    {
        hasPowder = true;
        powderType = type;

        if (powderVisual != null && powderRenderer != null)
        {
            // Set the material color of our powder sphere
            powderRenderer.material.color = color;
            // Turn on the powder visual
            powderVisual.SetActive(true);
        }
        Debug.Log("Scooped up " + powderType);
        TutorialManager.instance.OnPowderScooped(type);
    }

    // A helper function to reset the spoon
    private void ResetSpoon()
    {
        hasPowder = false;
        powderType = "";
        if (powderVisual != null)
        {
            powderVisual.SetActive(false);
        }
        Debug.Log("Deposited powder. Spoon is now empty.");
    }
}