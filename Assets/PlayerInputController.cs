using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float jumpForce = 5.0f;
    public PliersController pliersController;
    public Animator animator;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    private Rigidbody2D rb2D;
    private bool isGrounded;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();

        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("Animator component not found on the player object.");
            }
        }
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        float moveDirection = Input.GetAxis("Horizontal");
        rb2D.velocity = new Vector2(moveDirection * moveSpeed, rb2D.velocity.y);

        if (moveDirection > 0)
        {
            animator.SetTrigger("Right");
        }
        else if (moveDirection < 0)
        {
            animator.SetTrigger("Left");
        }
        else
        {
            animator.Play("idle");
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb2D.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (pliersController != null)
            {
                pliersController.HandleGrabRelease();
            }
            else
            {
                Debug.LogError("PliersController is not assigned in the Inspector.");
            }
        }
    }
}
