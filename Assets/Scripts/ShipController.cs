using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShipController : MonoBehaviour
{
    private GameManager gameManager; // GameManager para gestionar las balas

    public float movementSpeed = 5f; // Velocidad de movimiento de la nave
    public float rotationSpeed = 200f; // Velocidad de rotación de la nave

    public GameObject bulletPrefab; // Prefab del proyectil (imagen del canvas)
    public Transform firePoint; // Punto de origen del disparo en la nave
    public float bulletSpeed = 10f; // Velocidad del proyectil

    private Camera mainCamera; // Cámara principal
    private float cameraWidth; // Ancho de la cámara
    private float cameraHeight; // Alto de la cámara

    private Rigidbody2D rb; // Rigidbody de la nave
    private ICommand moveCommand; // Comando de movimiento

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Obtener el Rigidbody de la nave
        mainCamera = Camera.main; // Obtener la cámara principal
        CalculateCameraDimensions(); // Calcular las dimensiones de la cámara
        gameManager = GameManager.GetInstance(); // Obtener la instancia del GameManager

        moveCommand = new MoveCommand(rb, transform, movementSpeed); // Inicializar el comando de movimiento
    }

    private void Update()
    {
        float moveInput = Input.GetAxis("Vertical");
        moveCommand.Execute(moveInput); // Ejecutar el comando de movimiento con la entrada del jugador

        float rotationInput = Input.GetAxis("Horizontal");
        rb.angularVelocity = -rotationInput * rotationSpeed; // Aplicar rotación a la nave

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (gameManager.GetActiveBulletCount() < gameManager.maxActiveBullets)
            {
                Shoot(); // Disparar solo si no se alcanza el límite de balas activas
            }
        }

        RestrictPlayerMovement(); // Restringir el movimiento del jugador dentro de los límites de la cámara
    }

    private void CalculateCameraDimensions()
    {
        cameraHeight = 2f * mainCamera.orthographicSize; // Calcular el alto de la cámara
        cameraWidth = cameraHeight * mainCamera.aspect; // Calcular el ancho de la cámara
    }

    private void RestrictPlayerMovement()
    {
        Vector3 clampedPosition = transform.position; // Posición actual del jugador

        float minX = mainCamera.transform.position.x - cameraWidth / 2f; // Límite izquierdo de la cámara
        float maxX = mainCamera.transform.position.x + cameraWidth / 2f; // Límite derecho de la cámara
        float minY = mainCamera.transform.position.y - cameraHeight / 2f; // Límite inferior de la cámara
        float maxY = mainCamera.transform.position.y + cameraHeight / 2f; // Límite superior de la cámara

        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX); // Restringir la posición en el eje X
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY); // Restringir la posición en el eje Y

        transform.position = clampedPosition; // Actualizar la posición restringida del jugador
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation); // Instanciar un proyectil
        bullet.tag = "Bullet"; // Etiquetar el proyectil como "Bullet"

        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>(); // Obtener el Rigidbody del proyectil
        Vector2 bulletDirection = firePoint.up; // Calcular la dirección del proyectil
        bulletRb.velocity = bulletDirection * bulletSpeed; // Establecer la velocidad del proyectil

        Collider2D bulletCollider = bullet.GetComponent<Collider2D>(); // Obtener el Collider del proyectil
        Collider2D enemyCollider = GetComponent<Collider2D>(); // Obtener el Collider del enemigo (nave)
        Physics2D.IgnoreCollision(bulletCollider, enemyCollider); // Ignorar la colisión entre el proyectil y la nave

        gameManager.AddBullet(bullet); // Agregar el proyectil a la lista de balas activas en el GameManager

        StartCoroutine(DestroyBulletAfterDelay(bullet, 3f)); // Iniciar la corutina para destruir el proyectil después de un tiempo
    }

    private IEnumerator DestroyBulletAfterDelay(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay); // Esperar el tiempo especificado

        gameManager.RemoveBullet(bullet); // Eliminar el proyectil de la lista de balas activas en el GameManager
        Destroy(bullet); // Destruir el proyectil
    }

    public interface ICommand
    {
        void Execute(float input); // Método para ejecutar el comando con una entrada de tipo float
    }

    public class MoveCommand : ICommand
    {
        private Rigidbody2D rb; // Rigidbody utilizado para el movimiento
        private Transform transform; // Transform utilizado para la dirección
        private float speed; // Velocidad del movimiento

        public MoveCommand(Rigidbody2D rb, Transform transform, float speed)
        {
            this.rb = rb; // Asignar el Rigidbody proporcionado
            this.transform = transform; // Asignar el Transform proporcionado
            this.speed = speed; // Asignar la velocidad proporcionada
        }

        public void Execute(float input)
        {
            rb.velocity = transform.up * input * speed; // Establecer la velocidad de movimiento basada en la dirección y la velocidad
        }
    }
}
