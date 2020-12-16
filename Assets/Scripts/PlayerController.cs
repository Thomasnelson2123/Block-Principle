using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float jumpForce = 400f;           // the amount of force the player jumps with
    [SerializeField] private Transform groundCheck;     // a transform that will be used to detect if the player is grounded
    [SerializeField] private LayerMask whatIsGround;    // layer mask that defines what ground is
    [Range(0, .3f)] [SerializeField] private float movementSmoothing = 0.05f;	// How much to smooth out the movement

    const float GroundCheckRadius = 0.25f;               // the radius for checking if the player is grounded
    private bool isGrounded;                            // boolean, if true then player is grounded, if false player is in the air
    private bool facingRight = true;                    // For determining which way the player is currently facing.

    private Rigidbody2D rb;
    private Vector3 v = Vector3.zero;

    public UnityEvent OnLandEvent;

    [SerializeField] private float fallMultiplier;
    [SerializeField] private float lowJumpMultiplier;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
        {
            OnLandEvent = new UnityEvent();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool wasGrounded = isGrounded;
        isGrounded = false;

        //The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, GroundCheckRadius, whatIsGround);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                isGrounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    public void Move(float move, bool jump)
    {
        // Move the character by finding the target velocity
        Vector3 targetVelocity = new Vector2(move * 10f, rb.velocity.y);
        // And then smoothing it out and applying it to the character
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref v, movementSmoothing);

        if (move < 0 && facingRight)
        {
            // flip left

            Flip();
        }
        if (move > 0 && !facingRight)
        {
            //flip right
            Flip();
        }

        // If the player should jump...
        if (isGrounded && jump)
        {
            // Add a vertical force to the player.
            isGrounded = false;
            rb.AddForce(new Vector2(0f, jumpForce));
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
