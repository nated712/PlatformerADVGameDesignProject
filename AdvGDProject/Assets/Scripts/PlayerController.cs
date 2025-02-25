using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    #region Variables

    [Header("Object References")]
    public CharacterController controller;

    [Header("UI Components")]
    [SerializeField] public TextMeshProUGUI speedText;
    [SerializeField] public Image dashIndicator;

    [Header("Movement Settings")]
    public float speed = 7f;
    public float jumpHeight = 6f;
    public float gravity = -26f;
    private Vector3 velocity;

    [Header("Sprint Variables")]
    public float sprintSpeedMultiplier = 2f;
    private bool isSprinting = false;

    [Header("Air Acceleration")]
    public float airSpeedMultiplier = 1.7f;
    public float airAccelerationRate = 1.8f;
    public float groundDecelerationRate = 4f;
    private float currentSpeedMultiplier = 1f;

    [Header("Dash Variables")]
    public float dashSpeedMultiplier = 3f;
    public float dashDuration = 0.3f;
    public float dashCooldown = 2.5f;
    private bool canDash = true;
    private bool isDashing = false;
    private float origGrav;


    [Header("Slope Sliding Settings")]
    public float slideAngleThreshold = 40f; // Angle above which sliding occurs



    [Header("GroundCheck Settings")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private bool isGrounded;

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

        if (Input.GetButton("Jump") && isGrounded && !isDashing)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * origGrav);
        }
    }

    void HandleSprint()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isDashing)
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
                Vector3 slideDirection = Vector3.ProjectOnPlane(Vector3.down, hit.normal).normalized;
                controller.Move(slideDirection * (Mathf.Abs(gravity) * .7f) * Time.deltaTime);
            }
        }
    }

    #endregion

    #region Dash Logic

    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        gravity = 0f;
        velocity.y = 0f;
        UpdateDashUI();

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
        gravity = origGrav;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
        UpdateDashUI();
    }

    #endregion

    #region UI Methods

    void UpdateDashUI()
    {
        if (dashIndicator != null)
        {
            dashIndicator.color = canDash ? Color.green : Color.red;
        }
    }

    void UpdateSpeedUI(float currentSpeed)
    {
        if (speedText != null)
        {
            speedText.text = "Speed: " + currentSpeed.ToString("F2");
        }
    }

    #endregion
}
