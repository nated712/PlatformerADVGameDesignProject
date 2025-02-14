
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Object References")]
    //reference charactercontroller from inspector    
    public CharacterController controller;

    [Header("UI Components")]
    [SerializeField] public TextMeshProUGUI speedText; // Reference to speed UI
    [SerializeField] public Image dashIndicator; //UI indicator for dashing

    [Header("Movement Settings")]
    
    public float speed = 7f; //base move speed
    public float jumpHeight = 6f;
    public float gravity = -23f;
    
    Vector3 velocity; //for gravity calculation
    

    [Header("Sprint Variables")]
    public float sprintSpeedMultiplier = 2f; // Speed multiplier when sprinting
    private bool isSprinting = false;

    [Header("Air Acceleration")]
    public float airSpeedMultiplier = 1.7f; // Max speed multiplier in air
    public float airAccelerationRate = 1.8f; // How quickly the speed increases
    public float groundDecelerationRate = 4f; // How quickly speed resets when grounded
    private float currentSpeedMultiplier = 1f;

    [Header("Dash Variables")]
    public float dashSpeedMultiplier = 3f;
    public float dashDuration = 0.3f;
    public float dashCooldown = 2.5f;
    private bool canDash = true;
    private bool isDashing = false;
    private float origGrav; //Store gravity 



    [Header("GroundCheck Settings")]
    
    public Transform groundCheck; //stores transform of groundCheck object in inspector
    
    public float groundDistance = 0.4f; //size of sphere to check if grounded
    
    public LayerMask groundMask; //stores layer that we will count as ground
    private bool isGrounded; //used to check is player is grounded



    void Start()
    {
        origGrav = gravity; // Store original gravity value
        UpdateDashUI();
    }

    // Update is called once per frame
    void Update()
    {
        //set up axis for movement
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        //check if player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask.value);

        //reset velocity if grounded
        if(isGrounded && velocity.y < 0){
            velocity.y = -2f;
        }

        //jump logic
        if(Input.GetButtonDown("Jump") && isGrounded && !isDashing){
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * origGrav);
        }

        //create vector storing movement based on where player is looking
        Vector3 move = transform.right * x + transform.forward * z;
        
        // Sprint logic (only if not dashing)
        if (Input.GetKeyDown(KeyCode.LeftControl) && move.magnitude > 0 && !isDashing){
            isSprinting = !isSprinting; // Toggle sprinting on/off
        }


        // Double-tap dash activation
        if (Input.GetKeyDown(KeyCode.LeftShift)){
            StartCoroutine(Dash());
            }


        // air acceleration
        if (!isGrounded && move.magnitude > 0){
            // Accelerate toward max air speed smoothly
            currentSpeedMultiplier = Mathf.Lerp(currentSpeedMultiplier, airSpeedMultiplier, airAccelerationRate * Time.deltaTime);
        }
        else{
            // Decelerate back to normal speed when on the ground
            currentSpeedMultiplier = Mathf.Lerp(currentSpeedMultiplier, 1f, groundDecelerationRate * Time.deltaTime);
        }

        // **Apply currentSpeedMultiplier correctly**
        float currentSpeed = speed * currentSpeedMultiplier;
    
        if (isDashing){
            currentSpeed *= dashSpeedMultiplier; // Apply dash speed
        }
        else if (isSprinting){
            currentSpeed *= sprintSpeedMultiplier; // Apply sprint speed
        }

        controller.Move(move.normalized * currentSpeed * Time.deltaTime); 

        // Gravity handling (only apply gravity if not dashing)
        if (!isDashing)
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);
    

        UpdateSpeedUI(currentSpeed); // Update speed UI

    }

    IEnumerator Dash()
    {
        if(!canDash){
            yield break;
        }
        canDash = false;
        isDashing = true;
        gravity = 0f; // Disable gravity during dash
        velocity.y = 0f; // Stop any vertical movement
        UpdateDashUI();

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
        gravity = origGrav; // Restore gravity after dash

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
        UpdateDashUI();
    }

        void UpdateDashUI(){
        if (dashIndicator != null)
        {
            dashIndicator.color = canDash ? Color.green : Color.red;
        }
    }
        void UpdateSpeedUI(float currentSpeed){
        if (speedText != null){
            speedText.text = "Speed: " + currentSpeed.ToString("F2"); // Display speed with two decimals
        }
    }

}
