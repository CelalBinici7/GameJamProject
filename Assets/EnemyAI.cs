using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;          
    public float moveSpeed = 2f;      
    public float attackRange = 1.5f;  
    public LayerMask groundLayer;     

    private Animator animator;
    private Rigidbody2D rb;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (player == null) return;

        if (IsOnSamePlatform())
        {
            float distance = Vector2.Distance(transform.position, player.position);

            if (distance > attackRange)
            {
                animator.SetBool("isAttacking", false);
                animator.SetFloat("Speed", Mathf.Abs(moveSpeed));

                Vector2 direction = (player.position - transform.position).normalized;
                rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);

                if (direction.x != 0)
                    transform.localScale = new Vector3(Mathf.Sign(direction.x), 1, 1);
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
                animator.SetFloat("Speed", 0f);
                animator.SetBool("isAttacking", true);
            }
        }
    }

    private bool IsOnSamePlatform()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, groundLayer);
        return hit.collider != null;
    }
}
