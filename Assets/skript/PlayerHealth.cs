using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 1;
    public GameManager gameManager;

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            gameManager.RestartGame();
        }
    }
}