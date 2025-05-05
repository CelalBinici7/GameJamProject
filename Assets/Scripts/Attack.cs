using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform firePoint;
    private CharacterMovement movement;

    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioSource audioSource;

    Animator animator;

    private bool isAttacking = false; 
    private bool canAttack = true;    

    private void Start()
    {
        movement = GetComponent<CharacterMovement>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
       
        if (Input.GetMouseButtonDown(0)&&!isAttacking)
        {
            isAttacking = true;         
            animator.SetTrigger("attack");
            Invoke(nameof(EndAttack), 0.5f);
        }
      
    }

    void EndAttack()
    {
        isAttacking = false;
    }
    void Shoot()
    {
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
        arrow.GetComponent<Arrow>().SetDirection(movement.facingRight);

       
        if (audioSource && shootSound)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }
  
}
