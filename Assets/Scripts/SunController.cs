/**
 * SunController Class
 * -------------------
 * This class controls the movement and visual aspects of a sun in a Unity scene,
 * providing a day-night cycle effect.
 * 
 * Public Variables:
 * - dayDuration: Time in seconds for one full day-night cycle.
 * - skyboxMaterial: The skybox material used to represent the sky.
 * 
 * Private Variables:
 * - sun: Reference to the Light component representing the sun.
 * 
 * Methods:
 * - Start(): Called when the script is first initialized. Sets up initial values.
 * - Update(): Called every frame. Updates the sun's position and adjusts visual elements.
 * - UpdateSkybox(float angle): Updates the skybox color based on the time of day.
 * 
 * Usage:
 * Attach this script to a GameObject with a Light component to simulate a day-night cycle.
 * Adjust the 'dayDuration' and 'skyboxMaterial' variables in the Unity Editor for customization.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunController : MonoBehaviour
{
    [SerializeField]
    private float dayDuration = 100f; // Time in seconds for one full day-night cycle.

    public Material skyboxMaterial; // The skybox material.

    private Light sun;

    /**
     * Start Method
     * ------------
     * Initializes the script by setting up the reference to the sun's Light component
     * and applying the specified skybox material.
     */
    private void Start()
    {
        sun = GetComponent<Light>();
        RenderSettings.skybox = skyboxMaterial;
    }

    /**
     * Update Method
     * -------------
     * Called every frame. Updates the sun's position and adjusts visual elements
     * to create a day-night cycle effect.
     */
    private void Update()
    {
        // Calculate the angle based on the time passed.
        float angle = (Time.time / dayDuration) * 360f;

        // Set the sun's rotation to create the rising and setting effect.
        sun.transform.rotation = Quaternion.Euler(new Vector3(angle, 0, 0));

        // Adjust other visual elements like the skybox based on the time of day.
        UpdateSkybox(angle);
    }

    /**
     * UpdateSkybox Method
     * --------------------
     * Updates the skybox color based on the time of day, creating a gradual transition
     * between day and night colors.
     * 
     * Parameters:
     * - angle: The current angle of the sun in the day-night cycle.
     */
    private void UpdateSkybox(float angle)
    {
        // Define the duration of a full day-night cycle in degrees.
        float fullCycle = 360f;

        // Calculate a value between 0 and 1 based on the current angle and the full cycle.
        float t = Mathf.Repeat(angle / fullCycle, 1f);

        // Use the lerp value to blend between different colors for day and night.
        Color dayColor = new Color(0.5f, 0.5f, 1f);
        Color nightColor = new Color(0f, 0f, 0.2f);
        RenderSettings.skybox.SetColor("_Tint", Color.Lerp(dayColor, nightColor, t));
    }
}
