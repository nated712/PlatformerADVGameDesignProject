
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Object References")]
    //reference charactercontroller from inspector    
    public CharacterController controller;


    [Header("Movement Settings")]
    
    public float speed = 6f; //base move speed
    public float jumpHeight = 6f;
    public float gravity = -20f;
    
    Vector3 velocity; //for gravity calculation
    

    [Header("Sprint Variables")]
    public float sprintSpeedMultiplier = 1.5f; // Speed multiplier when sprinting
    private bool isSprinting = false;


    [Header("Dash Variables")]
    public float dashSpeedMultiplier = 3f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 3f;
    private bool canDash = true;
    private bool isDashing = false;
    private float origGrav; //Store gravity 

    // Double-tap dash variables
    private float lastDashPressTime = -1f;
    public float doubleTapTimeThreshold = 0.3f; // Max time between taps

    
    [SerializeField] public Image dashIndicator; //UI indicator for dashing
    

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
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

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
        isSprinting = Input.GetKey(KeyCode.LeftShift) && move.magnitude > 0 && !isDashing;


        // Double-tap dash activation
        if (Input.GetKeyDown(KeyCode.LeftShift)){
            if (Time.time - lastDashPressTime < doubleTapTimeThreshold && canDash && move.magnitude > 0){
                StartCoroutine(Dash());
            }
            lastDashPressTime = Time.time;
        }

        
        // Determine current speed
        float currentSpeed = speed;
        if (isDashing){
            currentSpeed *= dashSpeedMultiplier; // Apply dash speed
        }
        else if (isSprinting){
            currentSpeed *= sprintSpeedMultiplier; // Apply sprint speed
        }


        controller.Move(move * currentSpeed * Time.deltaTime);

        // Gravity handling (only apply gravity if not dashing)
        if (!isDashing)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        controller.Move(velocity * Time.deltaTime);
    
    }

    IEnumerator Dash()
    {
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

}
