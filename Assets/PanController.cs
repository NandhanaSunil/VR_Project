using UnityEngine;

public class PanController : MonoBehaviour
{
    // This variable will be true only when the pan is on a stove burner.
    public bool isOnStove { get; private set; } = false;

    // This public function will be called by the stove socket to update our status.
    public void SetStoveState(bool onStove)
    {
        isOnStove = onStove;
        Debug.Log("Pan 'isOnStove' state is now: " + isOnStove); // For testing
    }
}