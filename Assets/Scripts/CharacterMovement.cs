using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class CharacterMovement : MonoBehaviour
{
    public static CharacterMovement instance;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpForce = 5f;
    private Rigidbody2D rb;
    private Animator animator;
    public bool grounded;
   
    private bool jumping;
    Vector2 movementInput;
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask whatIsGround;
    public bool facingRight = true;
    private float previousXInput = 0f;
    [SerializeField] private int maxJumpCount = 1; 
    public int currentJumpCount = 0;

    [SerializeField] private AudioSource footstepAudioSource;
    [SerializeField] private AudioClip footstepClip;
    [SerializeField] private float footstepInterval = 0.4f; 

    private bool isPlayingFootsteps = false;

    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioSource jumpaudioSource;

    float clampVelocity;
    public GameObject StartPosition;

    private void Awake()
    {
        instance = this;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
      
    }
    void Start()
    {
        transform.position = StartPosition.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float currentXInput = Input.GetAxisRaw("Horizontal");

        if (currentXInput != 0)
        {
            previousXInput = currentXInput; 
        }
        movementInput = new Vector2(currentXInput, movementInput.y).normalized;
        
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        if (Input.GetKeyDown(KeyCode.Space) )
        {
            currentJumpCount++;
            if ( currentJumpCount < maxJumpCount)
            {
                animator.SetTrigger("jump");
                jumping = true;
                if (jumpaudioSource && jumpSound)
                {
                    jumpaudioSource.PlayOneShot(jumpSound);
                }
            }
         
            
        }
        if (grounded) {
            animator.SetTrigger("Grounded");
            currentJumpCount = 0;
        }

        if (movementInput.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (movementInput.x < 0 && facingRight)
        {
            Flip();
        }
        animator.SetBool("walk", Mathf.Abs(movementInput.x) > 0.1f);

      //  playFootEffect();



    }
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(speed,rb.linearVelocity.y );

        if (jumping)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); 
            jumping = false;
        }
      
      
        Vector2 movement = movementInput * speed;

      

        rb.linearVelocity = new Vector2(movement.x, rb.linearVelocity.y);
     
        animator.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocityX));
        
    }

    public void playFootEffect()
    {
        if (Mathf.Abs(movementInput.x) > 0.1f && grounded)
        {
            if (!isPlayingFootsteps)
            {
                StartCoroutine(PlayFootsteps());
            }
        }
        else
        {
            if (isPlayingFootsteps)
            {
                isPlayingFootsteps = false;
                StopCoroutine(PlayFootsteps());
            }
        }
    }
    private IEnumerator PlayFootsteps()
    {
        isPlayingFootsteps = true;
        while (isPlayingFootsteps)
        {
            footstepAudioSource.PlayOneShot(footstepClip);
            yield return new WaitForSeconds(footstepInterval);
        }
    }

    
}
