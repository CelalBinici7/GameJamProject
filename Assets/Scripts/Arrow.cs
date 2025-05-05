using Ilumisoft.HealthSystem;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 10f;
    public float maxDistance = 15f;
    public int damage = 20;

    private Vector2 direction = Vector2.right;
    private Vector2 startPos;
    public Animator animator;
    private bool canMove;

    [SerializeField] private float rayLength = 0.1f;
    [SerializeField] private Transform arrowTip;
    public bool facingRightArrow;

  

    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioSource audioSource;


    private void Start()
    {
        canMove = true;
        startPos = transform.position;
        animator = GetComponent<Animator>();
    }

    public void SetDirection(bool facingRight)
    {
        direction = facingRight ? Vector2.right : Vector2.left;

        // Sprite yönünü çevir
        Vector3 scale = transform.localScale;
        scale.x = facingRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
        facingRightArrow = facingRight;
    }

    private void Update()
    {
        if (canMove)
        {
           transform.Translate(direction * speed * Time.deltaTime);
           
            //  CheckHit();
        }

        if (Vector2.Distance(startPos, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void CheckHit()
    {
        Vector2 rayDirection = facingRightArrow ? Vector2.right : Vector2.left;

        RaycastHit2D hit = Physics2D.Raycast(arrowTip.position, rayDirection, rayLength, LayerMask.GetMask("Enemy"));

        Debug.DrawRay(arrowTip.position, rayDirection * rayLength, Color.red);

        if (hit.collider != null)
        {
            HealthSystem target = hit.collider.GetComponent<HealthSystem>();
            if (target != null)
            {
                // Hasar ver
                target.TakeDamage(damage);
                animator.SetTrigger("hit");

                // Oku düþmana yapýþtýr
                transform.parent = hit.collider.transform;
                canMove = false;
                GetComponent<Collider2D>().enabled = false;

                // Artýk tekrar iþlem yapýlmamasý için devre dýþý býrak
                enabled = false;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyHealth target = collision.GetComponent<EnemyHealth>();
            if (target != null)
            {
                target.TakeDamage(damage);
                animator.SetTrigger("hit");

                transform.parent = collision.transform;
                canMove = false;
                GetComponent<Collider2D>().enabled = false;

                enabled = false;
                if (hitSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(hitSound);
                }
            }
        }
       
    }
    public void DisableArrow()
    {
        Destroy(gameObject); 
    }
 

}
