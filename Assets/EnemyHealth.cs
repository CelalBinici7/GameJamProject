using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("UI (Optional)")]
    public Slider healthBarSlider;

    private Animator animator;
    private Collider2D hitCollider;
    private Rigidbody2D rb;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        hitCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        if (healthBarSlider != null)
        {
            healthBarSlider.maxValue = maxHealth;
            healthBarSlider.value = currentHealth;
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthBarSlider != null)
            healthBarSlider.value = currentHealth;

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        if (animator != null)
            animator.SetTrigger("Death");

        if (hitCollider != null)
            hitCollider.enabled = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;  
            rb.isKinematic = true;       
        }

        Destroy(gameObject, 1f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerAttack"))
        {
            TakeDamage(20);
        }
    }
}
