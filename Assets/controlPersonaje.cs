using UnityEngine;
using UnityEngine.InputSystem;  // Necesario para usar el nuevo Input System
using System.Collections;

public class ControlPersonaje : MonoBehaviour
{
    public float velocidad = 3f;
    public Rigidbody2D myrigidBody2D;
    public bool facingRight;

    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingCooldown = 0.5f;  // Tiempo de espera entre dashes (código corregido)
    private float dashingTime = 0.2f;

    private Vector2 movimiento;  // Variable para almacenar el movimiento horizontal

    // Llamado cuando se recibe input del movimiento (configurado en el nuevo Input System)
    void OnMove(InputValue value)
    {
        movimiento = value.Get<Vector2>();  // Guarda el valor de movimiento (eje X y Y)
    }

    void Update()
    {
        // Detectar si Shift izquierdo está presionado con el nuevo Input System
        if (Keyboard.current.leftShiftKey.isPressed && canDash)
        {
            StartCoroutine(Dash());  // Inicia el Dash
        }

        // Si no estamos dashing, procesamos el movimiento
        if (!isDashing)
        {
            ProcesarMovimiento();

            // Movimiento hacia la derecha con la tecla "D"
            if (Keyboard.current.dKey.isPressed)
            {
                myrigidBody2D.linearVelocity = new Vector2(velocidad, myrigidBody2D.linearVelocity.y);  // Mueve al personaje a la derecha
                facingRight = true;
            }

            // Movimiento hacia la izquierda con la tecla "A"
            if (Keyboard.current.aKey.isPressed)
            {
                myrigidBody2D.linearVelocity = new Vector2(-velocidad, myrigidBody2D.linearVelocity.y);  // Mueve al personaje a la izquierda
                facingRight = false;
            }

            // Movimiento hacia abajo con la tecla "S"
            if (Keyboard.current.sKey.isPressed)
            {
                myrigidBody2D.linearVelocity = new Vector2(myrigidBody2D.linearVelocity.x, -velocidad);  // Mueve al personaje hacia abajo
            }
        }
    }

    void ProcesarMovimiento()
    {
        // Lógica de movimiento horizontal (puede ser a la izquierda o derecha)
        myrigidBody2D.linearVelocity = new Vector2(movimiento.x * velocidad, myrigidBody2D.linearVelocity.y);
    }

    private IEnumerator Dash()
    {
        float localDashingPower;
        // Cambia la dirección del dash según la orientación del personaje
        if (!facingRight)
        {
            localDashingPower = -dashingPower;  // Dash hacia la izquierda si no está mirando a la derecha
        }
        else
        {
            localDashingPower = dashingPower;  // Dash hacia la derecha
        }

        canDash = false;  // Deshabilita el dash para evitar usarlo mientras está activo
        isDashing = true; // Establece que estamos haciendo un dash

        // Guardamos la gravedad original para restaurarla después
        float ogGravity = myrigidBody2D.gravityScale;
        myrigidBody2D.gravityScale = 0; // Desactivamos la gravedad durante el dash

        // Establece la velocidad del dash (sin gravedad, solo movimiento horizontal)
        myrigidBody2D.linearVelocity = new Vector2(localDashingPower, 0f);

        // Esperamos el tiempo del dash
        yield return new WaitForSeconds(dashingTime);

        // Restauramos la gravedad original
        myrigidBody2D.gravityScale = ogGravity;
        isDashing = false;  // Terminamos el dash

        // Esperamos el tiempo de cooldown antes de permitir otro dash
        yield return new WaitForSeconds(dashingCooldown);

        canDash = true;  // Ahora podemos hacer otro dash
    }
}