/**
 * Interactable_Chest Class
 * ------------------------
 * This class defines the behavior of an interactable chest in a Unity scene.
 * It allows the player to open the chest by pressing the "E" key when in range.
 * The chest lid rotates and translates upon opening.
 * 
 * Public Variables:
 * - lid: The Transform of the chest lid.
 * - openingSpeed: The speed at which the chest lid opens.
 * - interactionRange: The range at which the player can interact with the chest.
 * - interactableLayer: The layer which defines interactable objects.
 * 
 * Private Variables:
 * - isOpen: Tracks whether the chest is currently open.
 * 
 * Methods:
 * - Start(): Called when the script is first initialized. No initialization needed in this script.
 * - Update(): Called every frame. Checks if the player is in range and presses the interaction key.
 * - IsPlayerInRange(): Checks if the player is within range to interact with the chest.
 * - OpenLid(): Opens the chest lid by rotating and translating it.
 * - OnDrawGizmos(): Visualizes the interaction range with a ray in the Scene view.
 * 
 * Usage:
 * Attach this script to a GameObject representing an interactable chest in a Unity scene.
 * Customize the public variables in the Unity Editor for specific chest properties.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_Chest : MonoBehaviour
{
    // The Transform of the chest lid.
    public Transform lid;

    // The speed at which the chest lid opens.
    public float openingSpeed = 2.0f;

    // The range at which the player can interact with the chest.
    public float interactionRange = 10.0f;

    // The layer which defines interactable objects.
    public LayerMask interactableLayer;

    // Tracks whether the chest is currently open.
    private bool isOpen = false;

    /**
     * Update Method
     * --------------
     * Called every frame. Checks if the player is in range and presses the interaction key.
     */
    void Update()
    {
        if (IsPlayerInRange() && Input.GetKeyDown(KeyCode.E))
        {
            OpenLid();
        }
    }

    /**
     * IsPlayerInRange Method
     * -----------------------
     * Checks if the player is within range to interact with the chest.
     * 
     * Returns:
     * True if the player is in range, false otherwise.
     */
    bool IsPlayerInRange()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactionRange, interactableLayer))
        {
            if (hit.collider.gameObject == gameObject)
            {
                return true;
            }
        }

        return false;
    }

    /**
     * OpenLid Method
     * ---------------
     * Opens the chest lid by rotating and translating it.
     */
    void OpenLid()
    {
        if (!isOpen)
        {
            lid.Rotate(Vector3.up, 30f * openingSpeed);
            lid.Translate(Vector3.back * 0.33f);

            isOpen = true;

            Debug.Log("Chest opened");
        }
    }

    /**
     * OnDrawGizmos Method
     * --------------------
     * Visualizes the interaction range with a ray in the Scene view.
     */
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * interactionRange);
    }
}
