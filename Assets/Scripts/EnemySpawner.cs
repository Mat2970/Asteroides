using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab del enemigo
    public float spawnInterval = 2f; // Intervalo de tiempo entre cada spawn
    public float enemySpeed = 5f; // Velocidad de movimiento del enemigo

    private Camera mainCamera; // Referencia a la cámara

    private float cameraWidth;
    private float cameraHeight;

    private List<GameObject> enemyPool = new List<GameObject>(); // Pool de enemigos disponibles
    private List<GameObject> activeEnemies = new List<GameObject>(); // Lista de enemigos activos

    private void Start()
    {
        mainCamera = Camera.main;

        CalculateCameraDimensions();

        // Llenar el pool con enemigos
        FillEnemyPool();

        // Iniciar la rutina para spawnear enemigos
        StartCoroutine(SpawnEnemies());
    }

    private void FillEnemyPool()
    {
        // Crear varios enemigos y añadirlos al pool
        for (int i = 0; i < 10; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, Vector2.zero, Quaternion.identity);
            enemy.SetActive(false);
            enemyPool.Add(enemy);
        }
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // Obtener un enemigo del pool
            GameObject enemy = GetPooledEnemy();

            // Configurar la posición y velocidad del enemigo
            Vector2 spawnPosition = GetRandomSpawnPosition();
            Vector2 targetPosition = GetRandomTargetPosition();
            enemy.transform.position = spawnPosition;
            enemy.GetComponent<EnemyMovement>().SetMovement(targetPosition, enemySpeed);

            // Activar el enemigo y agregarlo a la lista de enemigos activos
            enemy.SetActive(true);
            activeEnemies.Add(enemy);

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private GameObject GetPooledEnemy()
    {
        // Buscar un enemigo inactivo en el pool
        foreach (GameObject enemy in enemyPool)
        {
            if (!enemy.activeInHierarchy)
            {
                return enemy;
            }
        }

        // Si no se encontró un enemigo inactivo, crear uno nuevo y añadirlo al pool
        GameObject newEnemy = Instantiate(enemyPrefab, Vector2.zero, Quaternion.identity);
        newEnemy.SetActive(false);
        enemyPool.Add(newEnemy);

        return newEnemy;
    }

    // Genera una posición aleatoria en uno de los bordes de la cámara para el spawn del enemigo
    private Vector2 GetRandomSpawnPosition()
    {
        float randomX = 0f;
        float randomY = 0f;

        // Generar un lado aleatorio de la cámara
        int randomEdge = Random.Range(0, 4);

        switch (randomEdge)
        {
            case 0: // Borde superior
                randomX = Random.Range(mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).x, mainCamera.ViewportToWorldPoint(new Vector3(1f, 0f, 0f)).x);
                randomY = mainCamera.ViewportToWorldPoint(new Vector3(0f, 1f, 0f)).y;
                break;
            case 1: // Borde inferior
                randomX = Random.Range(mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).x, mainCamera.ViewportToWorldPoint(new Vector3(1f, 0f, 0f)).x);
                randomY = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).y;
                break;
            case 2: // Borde izquierdo
                randomX = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).x;
                randomY = Random.Range(mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).y, mainCamera.ViewportToWorldPoint(new Vector3(0f, 1f, 0f)).y);
                break;
            case 3: // Borde derecho
                randomX = mainCamera.ViewportToWorldPoint(new Vector3(1f, 0f, 0f)).x;
                randomY = Random.Range(mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).y, mainCamera.ViewportToWorldPoint(new Vector3(0f, 1f, 0f)).y);
                break;
        }

        Vector2 spawnPosition = new Vector2(randomX, randomY);
        return spawnPosition;
    }

    // Genera una posición aleatoria en uno de los bordes de la cámara como destino para el enemigo
    private Vector2 GetRandomTargetPosition()
    {
        float randomX = 0f;
        float randomY = 0f;

        // Generar un lado aleatorio de la cámara
        int randomEdge = Random.Range(0, 4);

        switch (randomEdge)
        {
            case 0: // Borde superior
                randomX = Random.Range(mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).x, mainCamera.ViewportToWorldPoint(new Vector3(1f, 0f, 0f)).x);
                randomY = mainCamera.ViewportToWorldPoint(new Vector3(0f, 1f, 0f)).y;
                break;
            case 1: // Borde inferior
                randomX = Random.Range(mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).x, mainCamera.ViewportToWorldPoint(new Vector3(1f, 0f, 0f)).x);
                randomY = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).y;
                break;
            case 2: // Borde izquierdo
                randomX = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).x;
                randomY = Random.Range(mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).y, mainCamera.ViewportToWorldPoint(new Vector3(0f, 1f, 0f)).y);
                break;
            case 3: // Borde derecho
                randomX = mainCamera.ViewportToWorldPoint(new Vector3(1f, 0f, 0f)).x;
                randomY = Random.Range(mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).y, mainCamera.ViewportToWorldPoint(new Vector3(0f, 1f, 0f)).y);
                break;
        }

        Vector2 targetPosition = new Vector2(randomX, randomY);
        return targetPosition;
    }

    // Calcula las dimensiones de la cámara en base a su tamaño ortográfico y aspecto
    private void CalculateCameraDimensions()
    {
        cameraHeight = 2f * mainCamera.orthographicSize;
        cameraWidth = cameraHeight * mainCamera.aspect;
    }
}