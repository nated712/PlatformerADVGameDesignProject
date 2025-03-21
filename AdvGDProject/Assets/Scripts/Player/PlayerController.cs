using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Timers;

public class PlayerController : MonoBehaviour
{
    #region Variables

    [Header("Object References")]
    public CharacterController controller;

    [Header("UI Components")]
    [SerializeField] public TextMeshProUGUI speedText;
    [SerializeField] public Image dashIndicator;

    [Header("Movement Settings")]
    public float speed = 12f;
    public float jumpHeight = 5f;
    public float gravity = -28f;
    private Vector3 velocity;

    [Header("Sprint Variables")]
    public float sprintSpeedMultiplier = 1f;
    private bool isSprinting = false;

    [Header("Air Acceleration")]
    public float airSpeedMultiplier = 1.7f;
    public float airAccelerationRate = .2f;
    public float groundDecelerationRate = 3f;
    private float currentSpeedMultiplier = 1f;

    [Header("Dash Variables")]
    public float dashSpeedMultiplier = 2.7f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 2f;
    private bool canDash = true;
    private bool isDashing = false;
    private float origGrav;


    [Header("Slope Sliding Settings")]
    public float slideAngleThreshold = 40f; // Angle above which sliding occurs
    public bool isSliding = false;


    [Header("GroundCheck Settings")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private bool isGrounded;


    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip dashReadySound;

    #endregion

    void Start()
    {
        origGrav = gravity;
        UpdateDashUI();
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleSprint();
        HandleDash();
        HandleAirAcceleration();
        HandleSlopeSliding(); 
        ApplyGravity();
    }

    #region Movement Methods

void HandleMovement()
{
    float x = Input.GetAxisRaw("Horizontal");
    float z = Input.GetAxisRaw("Vertical");

    Vector3 move = transform.right * x + transform.forward * z;

    if (move.magnitude > 1) // Prevent diagonal speed increase
    {
        move.Normalize();
    }

    // Base speed with air acceleration multiplier
    float finalSpeed = speed * currentSpeedMultiplier;

    // Apply sprinting or dashing multiplier
    if (isDashing)
    {
        finalSpeed *= dashSpeedMultiplier;
    }
    else if (isSprinting)
    {
        finalSpeed *= sprintSpeedMultiplier;
    }

    // Move the player
    controller.Move(move.normalized * finalSpeed * Time.deltaTime);

    // Update the speed UI with the true current speed
    UpdateSpeedUI(finalSpeed);
}

    void HandleJump()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask.value);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetButton("Jump") && isGrounded && !isSliding)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * origGrav);
        }
    }

    void HandleSprint()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isDashing)
        {
            isSprinting = !isSprinting;
        }
    }

    void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    void HandleAirAcceleration()
    {
        if (!isGrounded && controller.velocity.magnitude > 0)
        {
            currentSpeedMultiplier = Mathf.Lerp(currentSpeedMultiplier, airSpeedMultiplier, airAccelerationRate * Time.deltaTime);
        }
        else
        {
            currentSpeedMultiplier = Mathf.Lerp(currentSpeedMultiplier, 1f, groundDecelerationRate * Time.deltaTime);
        }
    }

    void ApplyGravity()
    {
        if (!isDashing)
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);
    }

void HandleSlopeSliding()
{
    if (Physics.Raycast(groundCheck.position, Vector3.down, out RaycastHit hit, groundDistance + 1.5f))
    {
        float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);

        if (slopeAngle > slideAngleThreshold)
        {
            isSliding = true;
            isGrounded = false;

            // Get slide direction along the slope
            Vector3 slideDirection = Vector3.ProjectOnPlane(Vector3.down, hit.normal).normalized;

            // Ensure sliding happens even if the player is not moving
            float slideSpeed = Mathf.Max(controller.velocity.magnitude, speed * 1f); // Ensure a minimum speed

            // Apply sliding movement
            controller.Move(slideDirection * slideSpeed * Time.deltaTime);
            
            return;
        }
    }

    isSliding = false; // Reset sliding state when not on a slope
}

    #endregion

    #region Dash Logic

    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        
        UpdateDashUI();

        //Player takes damage when they dash
        PlayerHealthManager healthManager = GetComponent<PlayerHealthManager>();
        if (healthManager != null)
        {
            healthManager.TakeDamage(50);
        }

        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        float decelerationTime = 1f;
        float timeElapsed = 0f;
        //return player to their original speed so you dont lose the speed you had
        float storedMult = currentSpeedMultiplier + .2f;
        //DASH DECELERATION
        while(timeElapsed < decelerationTime){
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / decelerationTime;
            currentSpeedMultiplier = Mathf.Lerp(dashSpeedMultiplier, storedMult, t);
            yield return null;
        }
        currentSpeedMultiplier = storedMult;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
        UpdateDashUI();
        
        // Play dash-ready sound when cooldown ends
        if (audioSource != null && dashReadySound != null)
        {
            audioSource.PlayOneShot(dashReadySound);
        }

    }

    #endregion

    #region UI Methods

    void UpdateDashUI()
    {
        if (dashIndicator != null)
        {
            dashIndicator.color = canDash ? Color.yellow : Color.black;
        }
    }

    void UpdateSpeedUI(float currentSpeed)
    {
        if (speedText != null)
        {
            speedText.text = currentSpeed.ToString("F2");
        }
    }

    #endregion
}
