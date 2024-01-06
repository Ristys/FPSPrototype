/**
 * ParticleController Class
 * ------------------------
 * This class controls the behavior of a GameObject with a ParticleSystem component.
 * It automatically destroys the GameObject after the particle effect finishes its duration.
 * 
 * Private Variables:
 * - myParticleSystem: Reference to the ParticleSystem component attached to the GameObject.
 * 
 * Methods:
 * - Start(): Called when the script is first initialized. Sets up initial values and schedules
 *   the destruction of the GameObject after the particle effect's duration.
 * - DestroyGameObject(): Destroys the GameObject after the particle effect's duration.
 * 
 * Usage:
 * Attach this script to a GameObject with a ParticleSystem component in a Unity scene.
 * The GameObject will be automatically destroyed after the particle effect finishes.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    private ParticleSystem myParticleSystem;

    /**
     * Start Method
     * ------------
     * Initializes the script by obtaining a reference to the ParticleSystem component and
     * scheduling the destruction of the GameObject after the particle effect's duration.
     */
    void Start()
    {
        myParticleSystem = GetComponent<ParticleSystem>();

        // Particle effect is destroyed after finishing duration.
        Invoke("DestroyGameObject", myParticleSystem.main.duration);
    }

    /**
     * DestroyGameObject Method
     * -------------------------
     * Destroys the GameObject after the particle effect's duration.
     */
    void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
