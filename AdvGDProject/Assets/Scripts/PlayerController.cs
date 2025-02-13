
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Object References")]
    //reference charactercontroller from inspector    
    public CharacterController controller;


    [Header("Movement Settings")]
    //movespeed
    public float speed = 9f;
    public float jumpHeight = 5f;
    public float gravity = -18f;
    //for gravity calculation
    Vector3 velocity;
    
    
    [Header("GroundCheck Settings")]
    //stores transform of groundCheck object in inspector
    public Transform groundCheck;
    //size of sphere to check if grounded
    public float groundDistance = 0.4f;
    //store layer that we will count as ground
    public LayerMask groundMask;
    private bool isGrounded;


    [Header("Dash Variables")]
    public float dashSpeedMultiplier = 3f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 3f;
    private bool canDash = true;
    private bool isDashing = false;
    private float origGrav; //Store gravity 
    //UI indicator for dashing
    [SerializeField] public Image dashIndicator;
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
        
        
        // Dash activation
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && move.magnitude > 0)
        {
            StartCoroutine(Dash());
        }

        
        //Moves character controller with created vector
        float currentSpeed = isDashing ? speed * dashSpeedMultiplier : speed;
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
        velocity.y = 0f; // Stop any downward movement
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
