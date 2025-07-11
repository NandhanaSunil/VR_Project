using UnityEngine;
using UnityEngine.UI; // We need this line to work with UI elements
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [Header("Player and Teleport")]
    public GameObject vrPlayer; // Assign your VR Player Rig here in the Inspector
    public Transform sinkTeleportTarget;
    public Transform stoveTeleportTarget;
    public Transform mainMenuTeleportTarget; // A spot to stand when in the menu
    public Transform gardenTeleportTarget;  
    

    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject cookingMenuPanel;
    public GameObject boilingCompletedPanel;

    void Start()
    {
        // When the game starts, we want to show only the main menu.
        ShowMainMenu();
    }

    // --- UI Control Functions ---

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        cookingMenuPanel.SetActive(false);
        boilingCompletedPanel.SetActive(false);
        // Teleport the player to the menu viewing spot
        vrPlayer.transform.position = mainMenuTeleportTarget.position;
    }

    public void ShowCookingMenu()
    {
        mainMenuPanel.SetActive(false);
        Debug.Log("Showing Cooking Menu");
        cookingMenuPanel.SetActive(true);
    }

    public void GoToGarden()
    {
        // When the player goes to the garden, we should probably hide the main menu.
        mainMenuPanel.SetActive(false);

        // Teleport the player to the garden spot.
        if (vrPlayer != null && gardenTeleportTarget != null)
        {
            Debug.Log("Teleporting player to garden.");
            vrPlayer.transform.position = gardenTeleportTarget.position;
        }
        else
        {
            Debug.LogError("VR Player or Garden Teleport Target is not assigned in the GameManager!");
        }
    }
    // --- NEW FUNCTION for starting the omelette task ---
    public void StartOmeletteTask()
    {
        Debug.Log("Loading the Omelette Scene...");
        // The string name MUST exactly match your scene file name, without the ".unity" extension.
        SceneManager.LoadScene("OmletteScene"); 
    }
    // We will add more functions here later for boiling, etc.

    // We will add more functions here later for boiling, etc.
    public void StartCoffeeTask()
    {
        Debug.Log("Loading the Coffee Scene...");
        // The string name MUST exactly match your scene file name, without the ".unity" extension.
        SceneManager.LoadScene("Coffee");
    }
    public void StartJuiceTask()
    {
        Debug.Log("Loading the Juice Scene...");
        // The string name MUST exactly match your scene file name, without the ".unity" extension.
        SceneManager.LoadScene("FriesScene");
    }
}