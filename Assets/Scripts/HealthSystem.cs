using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class HealthSystem : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;
    public int damage;
    public bool canDamage;

    [Header("UI")]
    public Slider healthSlider;
    private float targetHealthPercent;
    private float lerpSpeed = 5f;       
    Animator animator;

    public GameObject LosePanel;
    
    private void Start()
    {
        currentHealth = maxHealth;
        targetHealthPercent = 1f;
        healthSlider.value = 1f;

        animator = GetComponent<Animator>();    
    }

  
    void Update()
    {
        if (healthSlider != null)
        {
            healthSlider.value = Mathf.Lerp(healthSlider.value, targetHealthPercent, Time.deltaTime * lerpSpeed);
        }

     
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(damage);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        targetHealthPercent = (float)currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Karakter öldü!");
      
        animator.Play("dead");
        
    }
    void rebirth()
    {
        targetHealthPercent = 1f;
        healthSlider.value = 1f;
        animator.SetBool("Die", false);

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("damage"))
        {
            // Aktif sahneyi yeniden yükle
            LosePanel.SetActive(true); 
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        LosePanel.SetActive(false);
        
    }

    // Eðer trigger collider kullanýyorsan þunu kullan:
   
}
