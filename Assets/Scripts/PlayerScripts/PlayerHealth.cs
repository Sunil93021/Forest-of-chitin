using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float deadDelay = 2f;
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

        StartCoroutine(DeadPlay());
        GetComponent<Rigidbody2D>().gravityScale = 0;
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponent<PlayerHealth>().enabled = false;
        // restart after few seconds

    }
    IEnumerator DeadPlay()
    {
        yield return new WaitForSecondsRealtime(deadDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
    }

}
