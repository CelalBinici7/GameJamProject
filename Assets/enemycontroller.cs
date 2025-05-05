using UnityEngine;

public class EnemyTestController : MonoBehaviour
{
    public float speed = 3f;

    private Animator animator;
    private Rigidbody2D rb;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);

        animator.SetFloat("Speed", Mathf.Abs(horizontal));

        if (horizontal != 0)
            transform.localScale = new Vector3(Mathf.Sign(horizontal), 1, 1);

        if (Input.GetKeyDown(KeyCode.Space))
            animator.SetBool("isAttacking", true);
        if (Input.GetKeyUp(KeyCode.Space))
            animator.SetBool("isAttacking", false);
    }
}
