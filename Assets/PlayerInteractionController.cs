// PlayerInteractionController.cs (Small change to call the new PickupObject)
using UnityEngine;

[RequireComponent(typeof(ObjectHolder))]
public class PlayerInteractionController : MonoBehaviour
{
    [SerializeField] private LayerMask grabbableLayer;
    [SerializeField] private float interactionRange = 5f;
    private ObjectHolder objectHolder;

    void Awake()
    {
        objectHolder = GetComponent<ObjectHolder>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (objectHolder.IsHoldingObject)
            {
                // Drop logic remains the same
                objectHolder.DropObject();
            }
            else
            {
                RaycastForPickup();
            }
        }
    }

    //void RaycastForPickup()
    //{
    //    // CHANGE IS HERE: We now use ScreenPointToRay for consistency
    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    RaycastHit hit;

    //    if (Physics.Raycast(ray, out hit, interactionRange))
    //    {
    //        if (Physics.Raycast(ray, out hit, interactionRange, grabbableLayer))
    //            {
    //                // --- NO TAG CHECK NEEDED ANYMORE ---
    //                // Because the raycast can only hit things on the grabbable layer,
    //                // we no longer need the CompareTag check.
    //                objectHolder.PickupObject(hit.transform.gameObject, hit);
    //            }
    //        else if (hit.transform.CompareTag("Interactable"))
    //        {
    //            if (hit.collider.TryGetComponent(out IInteractable interactable))
    //            {
    //                interactable.Interact();
    //            }
    //        }
    //    }
    //}

    void RaycastForPickup()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            // --- ADD THESE LOGS ---
            GameObject hitObject = hit.collider.gameObject;
            Debug.Log("Raycast hit: " + hitObject.name + " | Tag: " + hitObject.tag + " | Layer: " + LayerMask.LayerToName(hitObject.layer));
            // --- END OF LOGS ---

            if ((grabbableLayer.value & (1 << hitObject.layer)) > 0)
            {
                Debug.Log("Result: Object is on Grabbable layer. Picking up.");
                objectHolder.PickupObject(hitObject, hit);
            }
            else if (hitObject.CompareTag("Interactable"))
            {
                Debug.Log("Result: Object has Interactable tag. Trying to find component.");
                if (hit.collider.TryGetComponent(out IInteractable interactable))
                {
                    Debug.Log("SUCCESS: Found IInteractable component. Interacting!");
                    interactable.Interact();
                }
                else
                {
                    Debug.Log("FAILURE: Object has Interactable tag, but no IInteractable script was found or script is disabled!");
                }
            }
        }
    }
}