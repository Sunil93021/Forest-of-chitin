using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    public void TakeDamage(int damage)
    {
        animator.SetTrigger("Hit");
        maxHealth -= damage;
        if(maxHealth < 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player dead");
        // player dead animation
        animator.SetBool("IsDead", true);

        // restart after few seconds
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  

    }

}
