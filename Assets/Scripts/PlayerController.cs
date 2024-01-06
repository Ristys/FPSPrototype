/**
 * PlayerController Class
 * ----------------------
 * This class handles player movement, camera control, shooting, throwing grenades,
 * crouching, jumping, and interaction with objects in a Unity scene.
 * 
 * Public Variables:
 * - interactableLayer: Specifies objects that are interactable.
 * - lineRenderer: The line renderer used for visualizing hit-scan bullets.
 * - leftArm: Player's left arm.
 * - leftHand: The tip of the left arm.
 * - rightArm: Player's right arm.
 * - rightHand: The tip of the player's right arm.
 * - bulletPrefab: The bullet prefab object.
 * - grenadePrefab: The grenade prefab object.
 * - grenadeCharge: The UI element which visualizes the power of the grenade throw.
 * 
 * Private Variables:
 * - movementSpeed: Player's movement speed.
 * - sprintSpeedDelta: How much speed is added when the player is sprinting.
 * - mouseSensitivity: Look speed sensitivity.
 * - jumpForce: Force of player's jump.
 * - characterHeight: The player's height when standing.
 * - crouchHeight: The player's height when crouching.
 * - gravityMultiplier: Force of gravity pulling player to the ground.
 * - shotDuration: The amount of time it takes for the hit-scan bullet to reach its destination.
 * - maxHitscanRange: The range of the hit-scan weapon.
 * - grenadeLifetime: How long a grenade can exist before being destroyed.
 * - groundCheckRadius: How far to check if the player is on the ground.
 * - headCheckRadius: How far to check if the player can stand up after crouching.
 * - groundMask: Specifies what is considered the ground.
 * - interactionRange: How close the player needs to be to interact with something.
 * - grenadeChargeInstance: Holds the instance of the grenadeCharge prefab.
 * - isOnGround: Tracks whether the player is on the ground or not.
 * - isCrouching: Tracks whether the player is crouching or not.
 * - holdStartTime: When the throw grenade button was first held down.
 * - characterController: Holds the CharacterController component.
 * 
 * Methods:
 * - Start(): Initializes the script by setting up initial values and references.
 * - Update(): Called every frame. Handles player input and updates various functionalities.
 * - ToggleCrouch(): Toggles the crouching boolean.
 * - AdjustHeightForCrouch(): Adjusts the Character Controller's height based on current crouch state.
 * - ShootBullet(): Shoots a hit-scan bullet out of the left arm.
 * - VisualizeBullet(Vector3 startPosition, Vector3 endPosition): Visualizes the hit-scan bullet.
 * - DisableLineRenderer(): Disables the Line Renderer used for visualizing hit-scan bullets.
 * - ThrowGrenade(): Throws a grenade out of the right arm.
 * - CalculateThrowForceCoroutine(): Coroutine to calculate throw force over time.
 * - ApplyGrenadeForce(float force): Applies the calculated force to the grenade.
 * - IsPlayerInRange(): Checks if the player is within range and looking at an interactable object.
 * - OnDrawGizmos(): Visualizes where the player is looking and if they're in range/looking at an interactable object.
 * 
 * Usage:
 * Attach this script to a player GameObject in a Unity scene and customize the public variables
 * for movement, camera control, and interaction behaviors.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Player's movement speed.
    [SerializeField]
    private float movementSpeed = 5f;

    // How much speed is added when the player is sprinting.
    [SerializeField]
    private float sprintSpeedDelta = 7;

    // Look speed sensitivity.
    [SerializeField]
    private float mouseSensitivity = 2f;

    // Force of player's jump.
    [SerializeField]
    private float jumpForce = 150f;

    // The player's height when standing.
    [SerializeField]
    private float characterHeight = 2f;

    // The player's height when crouching.
    [SerializeField]
    private float crouchHeight = 0.5f;

    // Force of gravity pulling player to the ground.
    [SerializeField]
    private float gravityMultiplier = 15f;

    // The amount of time it takes for the hit-scan bullet to reach its destination.
    [SerializeField]
    private float shotDuration = 1f;

    // The range of the hit-scan weapon.
    [SerializeField]
    private float maxHitscanRange = 100f;

    // How long a grenade can exist before being destroyed.
    [SerializeField]
    private float grenadeLifetime = 1f;

    // How far we'll check to see if the player is on the ground.
    [SerializeField]
    private float groundCheckRadius = 0.5f;

    // How far we'll check to see if the player can stand up after crouching.
    [SerializeField]
    private float headCheckRadius = 0.5f;

    // Specifies what is considered the ground.
    [SerializeField]
    private LayerMask groundMask;

    // How close the player needs to be to interact with something.
    [SerializeField]
    private float interactionRange = 5f;


    // Specifies objects that are interactable.
    public LayerMask interactableLayer;

    // The line renderer used for visualizing the hit-scan bullets.
    public LineRenderer lineRenderer;

    // Player's left arm.
    public GameObject leftArm;

    // The tip of the left arm.
    public Transform leftHand;

    // Player's right arm.
    public GameObject rightArm;

    // The tip of the player's right arm.
    public Transform rightHand;

    // The bullet prefab object.
    public GameObject bulletPrefab;

    // The grenade prefab object.
    public GameObject grenadePrefab;

    // The UI element which visualizes the power of the grenade throw.
    public ChargePrefab grenadeCharge;


    // Holds the instance of the grenadeCharge prefab.
    private ChargePrefab grenadeChargeInstance;

    // Tracks whether the player is on the ground or not.
    private bool isOnGround;

    // Tracks whether the player is crouching or not.
    private bool isCrouching;

    // When the throw grenade button was first held down.
    private float holdStartTime;

    // Holds CharacterController component.
    private CharacterController characterController;

    void Start()
    {
        // Instantiate the grenadeCharge prefab.
        grenadeChargeInstance = Instantiate(grenadeCharge, rightHand.position + Vector3.up * 0.2f, Quaternion.identity);
        grenadeChargeInstance.transform.SetParent(transform);

        // Set up the Charge image in the grenadeCharge UI.
        grenadeChargeInstance.charge = grenadeChargeInstance.transform.Find("Charge").GetComponent<Image>();
        grenadeChargeInstance.charge.fillAmount = 0;

        // Get the character controller component.
        characterController = GetComponent<CharacterController>();

        // Lock the mouse to the game window.
        Cursor.lockState = CursorLockMode.Locked;

        // Player always starts on the ground.
        isOnGround = true;

        // Player always starts standing up.
        isCrouching = false;
    }

    void Update()
    {
        // Check if the player is currently on the ground.
        isOnGround = Physics.CheckSphere(transform.position - new Vector3(0f, 0.5f, 0f), groundCheckRadius, groundMask);

        if (IsPlayerInRange() && Input.GetKeyDown(KeyCode.E))
        {
            //Interact with the object.
        }

        // Shoot bullet out of left arm when left mouse is clicked.
        if (Input.GetMouseButtonDown(0))
        {
            ShootBullet();
        }

        // Throw grenade out of arm when right mouse is clicked.
        if (Input.GetMouseButtonDown(1))
        {
            ThrowGrenade();
        }

        // Player sprints when Left Shift is held down.
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            movementSpeed += sprintSpeedDelta;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            movementSpeed -= sprintSpeedDelta;
        }


        // Get keyboard (WASD) input.
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Get current direction so movement can be relative to where the player is looking.
        Vector3 forwardDirection = transform.forward;
        Vector3 rightDirection = transform.right;

        // Zero the Y value to prevent "flying".
        forwardDirection.y = 0f;
        rightDirection.y = 0f;

        // Normalize the camera-relative vectors.
        forwardDirection.Normalize();
        rightDirection.Normalize();


        // Calculate how to move based on inputs, relative to the camera.
        Vector3 movementVector = forwardDirection * verticalInput + rightDirection * horizontalInput;

        // Perform a jump if the player is on the ground and presses the jump button.
        if (isOnGround && Input.GetKeyDown("space"))
        {
            movementVector.y = Mathf.Sqrt(2f * jumpForce * Physics.gravity.magnitude);
        }

        // Pull the player back to the ground based on gravity and the gravity multiplyer.
        movementVector.y += Physics.gravity.y * gravityMultiplier * Time.deltaTime;

        // Toggle crouch on player input.
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleCrouch();
        }

        // Adjust height according to if player is crouched or not.
        AdjustHeightForCrouch();


        // Get mouse input.
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Rotate camera based on mouse input and mouse sensitivity.
        transform.Rotate(Vector3.up * mouseX * mouseSensitivity);

        // Apply movement and rotation to the character controller.
        characterController.Move(movementVector * movementSpeed * Time.deltaTime);

        Vector3 currentRotation = transform.eulerAngles;
        currentRotation.x -= mouseY * mouseSensitivity;
        transform.eulerAngles = currentRotation;

        float rotationX = transform.eulerAngles.x;
        rotationX = (rotationX > 180) ? rotationX - 360 : rotationX;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        transform.eulerAngles = new Vector3(rotationX, transform.eulerAngles.y, 0f);

    }

    // Toggle the crouching boolean.
    void ToggleCrouch()
    {
        isCrouching = !isCrouching;
    }

    // Adjust the Character Controller's height based on current crouch state.
    void AdjustHeightForCrouch()
    {
        if (isCrouching)
        {
            characterController.height = crouchHeight;
        }
        else
        {
            if (!Physics.CheckSphere(transform.position + new Vector3(0f, 2f, 0f), headCheckRadius, groundMask))
            {
                characterController.height = characterHeight;
            }
            else
            {
                Debug.Log("BONK!");
            }

        }
    }

    // Shoot hit-scan out of left arm.
    void ShootBullet()
    {
        // Cast a ray from the left hand position in the direction it's facing.
        Ray ray = new Ray(leftHand.position, leftHand.forward);
        RaycastHit hit;

        // Check if the ray hits anything within the specified range.
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {

            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("Enemy was hit with a bullet");

                // Access the AIController script attached to the GameObject with the collider.
                AIController ai = hit.collider.gameObject.GetComponent<AIController>();

                // Check if the script was found.
                if (ai != null)
                {
                    ai.TakeDamage(10);
                }
            }

            // Set the destination to the hit point.
            Vector3 destination = hit.point;
            VisualizeBullet(leftHand.position, destination);

        }
        else
        {
            Debug.Log("Missed enemy with bullet.");

            // Set the destination to a point along the ray at the maximum range.
            Vector3 destination = leftHand.position + leftHand.forward * maxHitscanRange;


            VisualizeBullet(leftHand.position, destination);
        }
    }

    // Visualize the hit-scan bullet.
    void VisualizeBullet(Vector3 startPosition, Vector3 endPosition)
    {
        // Enable Line Renderer.
        lineRenderer.enabled = true;

        // Set Line Renderer positions.
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, endPosition);

        // Disable Line Renderer after a short duration.
        Invoke("DisableLineRenderer", shotDuration);
    }

    void DisableLineRenderer()
    {
        // Disable Line Renderer
        lineRenderer.enabled = false;
    }

    // Throw grenade out of right arm.
    void ThrowGrenade()
    {
        // Start a coroutine to calculate throw force over time.
        StartCoroutine(CalculateThrowForceCoroutine());
    }

    // Coroutine to calculate throw force over time.
    IEnumerator CalculateThrowForceCoroutine()
    {
        // Maximum throw force.
        float maxThrowForce = 50f;

        // Minimum hold duration required for max throw force.
        float maxHoldDuration = 1.5f;

        // The force applied to the grenade when button is unpressed.
        float throwForce = 0f;

        // Record the start time when the button is initially pressed.
        float startTime = Time.time;

        // Calculate throw force while the button is held down.
        while (Input.GetMouseButton(1))
        {
            // Calculate the normalized throw force based on the elapsed time.
            float normalizedThrowForce = Mathf.Clamp01((Time.time - startTime) / maxHoldDuration);

            // Update the charge UI.
            grenadeChargeInstance.UpdateCharge(normalizedThrowForce, maxHoldDuration);

            // Calculate the actual throw force.
            throwForce = normalizedThrowForce * maxThrowForce;

            yield return null;
        }

        // Apply the force to the grenade after the button is released.
        ApplyGrenadeForce(throwForce);

        // Set the charge UI back to 0.
        grenadeChargeInstance.UpdateCharge(0, maxThrowForce);
    }

    // Apply the calculated force to the grenade.
    void ApplyGrenadeForce(float force)
    {
        // Instantiate grenade prefab object.
        GameObject grenade = Instantiate(grenadePrefab, rightHand.position, rightHand.rotation);

        // Get grenade's script.
        Grenade grenadeScript = grenade.GetComponent<Grenade>();

        // Subscribe to the grenade's OnDestroyEvent to handle any additional logic.
        grenadeScript.OnDestroyEvent += () => Debug.Log("Grenade exploded!");

        // Get grenade's rigidbody.
        Rigidbody grenadeRB = grenade.GetComponent<Rigidbody>();

        // Make sure the rigidbody exists before attempting to add force to it.
        if (grenadeRB != null)
        {
            // Apply the force to the grenade.
            grenadeRB.AddForce(rightHand.forward * force, ForceMode.Impulse);
        }

        // Destroy the grenade after its lifetime.
        Destroy(grenade, grenadeLifetime);
    }

    // Check if the player is within range and looking at an interactable object.
    bool IsPlayerInRange()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactionRange, interactableLayer))
        {
            if (hit.collider.gameObject.layer == 3)
            {
                return true;
            }
        }
        return false;
    }




    /*
    * GIZMO
    * VISUALIZATION
    * BELOW
    */

    private void OnDrawGizmos()
    {
        // Visualize where the player is looking and if they're in range/looking at an interactable object.
        if (IsPlayerInRange())
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }

        Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * interactionRange);
    }

    // Visualize ground check radius in Scene view
    /*
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position - new Vector3(0f, 0.5f, 0f), groundCheckRadius);
    }
    */
}
