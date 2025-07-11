// LiquidCollector.cs
using UnityEngine;

// This automatically adds the LiquidContainer script if it's missing.
[RequireComponent(typeof(LiquidContainer))]
public class LiquidCollector : MonoBehaviour
{
    // Reference to the container script on this same object
    private LiquidContainer container;

    [Tooltip("How much 'volume' each particle adds to the container.")]
    [SerializeField] private float liquidAmountPerParticle = 0.2f;

    void Awake()
    {
        // Get the component when the game starts
        container = GetComponent<LiquidContainer>();
    }

    /// <summary>
    /// This is a special Unity function. It's called when a particle
    /// from a system with 'Send Collision Messages' enabled hits this collider.
    /// </summary>
    void OnParticleCollision(GameObject otherParticleSystem)
    {
        // Tell the container to add liquid
        Debug.Log("Particles collided --- filling");
        container.AddLiquid(liquidAmountPerParticle);
    }
}