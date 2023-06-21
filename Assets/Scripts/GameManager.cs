using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text scoreText; // Referencia al componente Text utilizado para mostrar la puntuación
    public int score = 0; // La puntuación del jugador
    public List<GameObject> bullets; // Lista de balas activas
    public int maxActiveBullets = 5; // Límite máximo de balas activas

    // Declaración del evento para notificar cambios en el contador de balas
    public delegate void BulletCountChanged(int count);
    public event BulletCountChanged OnBulletCountChanged;

    private static GameManager instance; // Instancia única del GameManager
    public float restartDelay = 2f;
    public GameObject gameOverUI;
    private bool isGameOver = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this; // Establecer esta instancia como la instancia única
            DontDestroyOnLoad(gameObject); // Mantener el objeto GameManager al cargar nuevas escenas
        }
        else
        {
            Destroy(gameObject); // Si ya existe una instancia, destruir este objeto GameManager
        }
    }

    private void Start()
    {
        bullets = new List<GameObject>(); // Inicializar la lista de balas activas
    }

    public void AddBullet(GameObject bullet)
    {
        if (bullets.Count < maxActiveBullets)
        {
            bullets.Add(bullet); // Agregar la bala a la lista de balas activas
            UpdateBulletCount(); // Actualizar el contador de balas
        }
        else
        {
            Destroy(bullet); // Destruir la bala si se alcanza el límite máximo de balas activas
        }
    }

    public void RemoveBullet(GameObject bullet)
    {
        bullets.Remove(bullet); // Eliminar la bala de la lista de balas activas
        UpdateBulletCount(); // Actualizar el contador de balas
    }

    private void UpdateBulletCount()
    {
        int count = bullets.Count; // Obtener la cantidad actual de balas activas
        if (OnBulletCountChanged != null)
        {
            OnBulletCountChanged.Invoke(count); // Notificar a los suscriptores sobre el cambio en el contador de balas
        }
    }

    public int GetActiveBulletCount()
    {
        return bullets.Count; // Devolver la cantidad de balas activas
    }

    public static GameManager GetInstance()
    {
        return instance; // Devolver la instancia única del GameManager
    }

    public void AddPoints(int points)
    {
        score += points; // Actualizar la puntuación sumando los puntos proporcionados
    }

    private void Update()
    {
        scoreText.text = "Score: " + score.ToString(); // Actualizar el texto de la puntuación en el UI
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel(); // Reiniciar el nivel si el juego ha terminado y se presiona la tecla "R"
        }
    }

    public void GameOver()
    {
        if (!isGameOver)
        {
            isGameOver = true; // Marcar el juego como terminado
            gameOverUI.SetActive(true); // Mostrar la interfaz de fin de juego
            Time.timeScale = 0f; // Congelar el tiempo en el juego
        }
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f; // Reactivar el tiempo normal del juego
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reiniciar la escena actual
    }
}