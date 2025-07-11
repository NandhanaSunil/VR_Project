using UnityEngine;
using UnityEngine.SceneManagement; // Essential for loading scenes

public class SceneNavigation : MonoBehaviour
{
    /// <summary>
    /// This public function loads the main menu scene.
    /// The scene name must exactly match the file name in your project.
    /// </summary>
    public void GoToMainMenu()
    {
        // "SampleScene" is the name of your main menu scene file.
        // Change this string if your scene has a different name (e.g., "MainMenu").
        Debug.Log("Loading Main Menu Scene...");
        SceneManager.LoadScene("SampleScene");
    }

    // You can add more functions here later for other scenes
    // public void GoToLevel2()
    // {
    //     SceneManager.LoadScene("Level2");
    // }
}