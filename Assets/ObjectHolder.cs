// ObjectHolder.cs (Cursor Following Version)
using UnityEngine;

public class ObjectHolder : MonoBehaviour
{
    private GameObject heldObject;
    private Rigidbody heldObjectRb;
    private Camera mainCamera;

    [Header("Cursor Following Settings")]
    [Tooltip("How smoothly the object follows the cursor (higher is snappier).")]
    [SerializeField] private float followSpeed = 20f;
    [Tooltip("How far the object is held from the camera by default.")]
    [SerializeField] private float defaultHoldDistance = 3f;
    [Tooltip("How fast the object rotates with the scroll wheel.")]
    [SerializeField] private float rotationSpeed = 100f;

    private float holdDistance; // The current distance we are holding the object at

    public bool IsHoldingObject => heldObject != null;

    void Awake()
    {
        // Get a reference to the main camera
        mainCamera = Camera.main;
    }

    void Update()
    {
        // This is now the main logic loop for controlling the held object
        if (heldObject != null)
        {
            MoveObjectWithCursor();
            RotateObjectWithScrollWheel();
        }
    }

    void MoveObjectWithCursor()
    {
        // Create a ray from the camera going through the mouse position
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        // Calculate the target position along the ray
        Vector3 targetPosition = ray.GetPoint(holdDistance);

        // Calculate the force needed to move the object to the target position
        Vector3 moveDirection = (targetPosition - heldObject.transform.position);

        // Apply the force. Using ForceMode.VelocityChange provides a snappy, responsive feel.
        heldObjectRb.AddForce(moveDirection * followSpeed, ForceMode.VelocityChange);
    }

    void RotateObjectWithScrollWheel()
    {
        // Get input from the mouse scroll wheel
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput != 0)
        {
            // Rotate the object around the camera's forward axis (rolling it)
            heldObject.transform.Rotate(mainCamera.transform.forward, -scrollInput * rotationSpeed * Time.deltaTime, Space.World);
        }
    }

    public void PickupObject(GameObject obj, RaycastHit hit)
    {
        if (obj.GetComponent<Rigidbody>())
        {
            heldObjectRb = obj.GetComponent<Rigidbody>();
            heldObjectRb.useGravity = false;
            heldObjectRb.linearDamping = 20f; // Increase drag for more stability
            heldObjectRb.constraints = RigidbodyConstraints.FreezeRotation; // Initially freeze rotation

            // Calculate the initial distance to hold the object at
            holdDistance = hit.distance;

            heldObject = obj;
        }
    }

    public void DropObject()
    {
        if (heldObject == null) return;

        heldObjectRb.useGravity = true;
        heldObjectRb.linearDamping = 1f;
        heldObjectRb.constraints = RigidbodyConstraints.None; // Unfreeze rotation

        heldObject = null;
    }
}