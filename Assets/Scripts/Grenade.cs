/**
 * Grenade Class
 * -------------
 * This class represents a throwable grenade in a Unity scene. Upon destruction, it triggers an explosion event
 * and checks for damageable objects within its explosion radius. It also handles the instantiation of an explosion
 * particle effect.
 * 
 * Public Variables:
 * - explosionRadius: The radius in which the grenade's explosion will cause damage.
 * - grenadeExplosionPrefab: The grenade explosion particle effect.
 * 
 * Events:
 * - OnDestroyEvent: Event to be triggered when the grenade is destroyed.
 * 
 * Methods:
 * - OnDestroy(): Called when the GameObject is destroyed. Triggers the explosion event, checks for damage, and invokes the OnDestroyEvent.
 * - CheckForDamage(): Checks for damageable objects within the explosion's radius and applies damage if applicable.
 * - TriggerDestroyEvent(): Invokes the OnDestroyEvent with no delay.
 * - TriggerExplosion(): Triggers the explosion by instantiating the explosion particle effect.
 * 
 * Usage:
 * Attach this script to a GameObject representing a throwable grenade in a Unity scene.
 * Customize the public variables in the Unity Editor for specific grenade properties.
 * Subscribe to the OnDestroyEvent to handle any additional logic upon grenade destruction.
 */
using System;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    // The radius in which the grenade's explosion will cause damage.
    [SerializeField]
    private float explosionRadius = 3f;

    // The grenade explosion particle effect.
    public GameObject grenadeExplosionPrefab;

    // Event to be triggered when the grenade is destroyed.
    public event Action OnDestroyEvent;

    /**
     * OnDestroy Method
     * -----------------
     * Called when the GameObject is destroyed. Triggers the explosion event, checks for damage, and invokes the OnDestroyEvent.
     */
    void OnDestroy()
    {
        // Trigger the explosion event.
        TriggerExplosion();

        // Check for damageable objects within the explosion's radius.
        CheckForDamage();

        // Invoke the OnDestroyEvent with no delay.
        Invoke("TriggerDestroyEvent", 0);
    }

    /**
     * CheckForDamage Method
     * ----------------------
     * Checks for damageable objects within the explosion's radius and applies damage if applicable.
     */
    void CheckForDamage()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider collider in colliders)
        {
            // Check for enemies within the explosion radius.
            if (collider.CompareTag("Enemy"))
            {
                Debug.Log("Enemy was within damage radius");

                // Access the AIController script attached to the GameObject with the collider.
                AIController ai = collider.gameObject.GetComponent<AIController>();

                // Check if the script was found.
                if (ai != null)
                {
                    ai.TakeDamage(25);
                }
            }
        }
    }

    /**
     * TriggerDestroyEvent Method
     * ---------------------------
     * Invokes the OnDestroyEvent with no delay.
     */
    void TriggerDestroyEvent()
    {
        // Trigger the OnDestroyEvent if there are any subscribers.
        OnDestroyEvent?.Invoke();
    }

    /**
     * TriggerExplosion Method
     * ------------------------
     * Triggers the explosion by instantiating the explosion particle effect.
     */
    void TriggerExplosion()
    {
        // Check if the explosionPrefab is assigned.
        if (grenadeExplosionPrefab != null)
        {
            // Instantiate the explosion particle effect.
            Instantiate(grenadeExplosionPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Explosion prefab is not assigned in Grenade script.");
        }
    }
}
