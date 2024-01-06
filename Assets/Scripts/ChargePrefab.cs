/**
 * ChargePrefab Class
 * -------------------
 * This class represents a charge bar with a filled charge value.
 * It provides a method to update the charge bar based on the current and maximum charge values.
 * 
 * Public Variables:
 * - charge: The Image component representing the current charge value to be displayed as a white bar.
 * 
 * Methods:
 * - UpdateCharge(float currentCharge, float maxCharge): Updates the charge bar based on the current and maximum charge values.
 * 
 * Usage:
 * Attach this script to a GameObject with an Image component representing the charge bar in a Unity scene.
 * Customize the public variables in the Unity Editor for specific charge bar properties.
 * Call the UpdateCharge method to update the charge bar based on the current and maximum charge values.
 */
using UnityEngine;
using UnityEngine.UI;

public class ChargePrefab : MonoBehaviour
{
    // The Image component representing the current charge value to be displayed as a white bar.
    public Image charge;

    /**
     * UpdateCharge Method
     * ---------------------
     * Updates the charge bar based on the current and maximum charge values.
     * 
     * Parameters:
     * - currentCharge: The current charge value.
     * - maxCharge: The maximum charge value.
     */
    public void UpdateCharge(float currentCharge, float maxCharge)
    {
        Debug.Log("Charge updated");

        // Calculate the fill amount based on the current and maximum charge values.
        float fillAmount = currentCharge / maxCharge;

        // Update the fill amount of the charge bar.
        charge.fillAmount = fillAmount;
    }
}
