using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public bool isBoss;
    public GameManager gameManager;

    //public Slider healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        //healthBar.maxValue = maxHealth;
        //healthBar.value = currentHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        //healthBar.value = currentHealth;

        if (currentHealth <= 0)
        {
            if (isBoss)
            {
                gameManager.WinGame();
            }

            Destroy(gameObject);
        }
    }
}