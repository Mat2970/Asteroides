using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyMovement : MonoBehaviour
{
    private Vector2 targetPosition; // Destino del enemigo
    private float movementSpeed; // Velocidad de movimiento del enemigo
    public GameObject gameOverText;
    private float speedIncreaseInterval = 10f; // Intervalo de aumento de velocidad
    private float speedIncreaseAmount = 2f; // Cantidad de aumento de velocidad

    private float timer; // Temporizador para el aumento de velocidad
    public GameManager gameManager;
    public void SetMovement(Vector2 target, float speed)
    {
        targetPosition = target;
        movementSpeed = speed;
    }
    private void Start()
    {
        gameOverText.SetActive(false);
        gameManager = GameManager.GetInstance();
    }

    private void Update()
    {
        // Mueve gradualmente al enemigo hacia el destino
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);

        // Comprueba si el enemigo ha alcanzado el destino
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            this.gameObject.SetActive(false); // Destruye el enemigo
        }

        // Actualiza el temporizador
        timer += Time.deltaTime;

        // Comprueba si ha pasado el intervalo de aumento de velocidad
        if (timer >= speedIncreaseInterval)
        {
            timer = 0f; // Reinicia el temporizador
            IncreaseSpeed(); // Aumenta la velocidad de movimiento
        }
    }

    private void IncreaseSpeed()
    {
        movementSpeed += speedIncreaseAmount; // Aumenta la velocidad de movimiento
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // Destruye el enemigo
            this.gameObject.SetActive(false);
            gameManager.AddPoints(100);
            // Destruye la bala
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            gameManager.GameOver();
        }

    }

}
