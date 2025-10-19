using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.SceneManagement;

public class ControlPersonaje : MonoBehaviour
{
    public float velocidad = 5f;
    public Rigidbody2D myrigidBody2D;
    public bool facingRight;
    public Animator animator;
    public int vida = 30;
    private bool youDied = false;

    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingCooldown = 0.5f;
    private float dashingTime = 0.2f;
    private bool onFloor = false;
    private float jumpForce = 10f;
    private float tiempoEspera = 30f;

    private Vector2 movimiento;

    // Llamado cuando se recibe input del movimiento
    void OnMove(InputValue value)
    {
        movimiento = value.Get<Vector2>();
    }

    void Update()
    {
        if (youDied)
        {
            Invoke("CambiarEscenaLuegoDeMuerte", tiempoEspera);
        }

        if (Keyboard.current.leftShiftKey.isPressed && canDash)
        {
            StartCoroutine(Dash());
            velocidad = 7f;
        }
        else
        {
            velocidad = 5f;
        }

        // Si no estamos dashing, procesamos el movimiento
        if (!isDashing)
        {
            ProcesarMovimiento();
            float velocidadHorizontal = Mathf.Abs(myrigidBody2D.linearVelocity.x);
            animator.SetFloat("Speed", velocidadHorizontal);

            // Movimiento hacia la derecha con la tecla "D"
            if (Keyboard.current.rightArrowKey.isPressed)
            {
                myrigidBody2D.linearVelocity = new Vector2(velocidad, myrigidBody2D.linearVelocity.y);
                facingRight = true;
                animator.SetFloat("Speed", velocidad);
                transform.localScale = new Vector3(1, 1, 1);
            }

            // Movimiento hacia la izquierda con la tecla "A"
            if (Keyboard.current.leftArrowKey.isPressed)
            {
                myrigidBody2D.linearVelocity = new Vector2(-velocidad, myrigidBody2D.linearVelocity.y);
                facingRight = false;
                animator.SetFloat("Speed", velocidad);
                transform.localScale = new Vector3(-1, 1, 1);
            }

            // Movimiento hacia abajo con la tecla "S"
            if (Keyboard.current.downArrowKey.isPressed)
            {
                myrigidBody2D.linearVelocity = new Vector2(myrigidBody2D.linearVelocity.x, -velocidad);
            }

            if (onFloor && Keyboard.current.zKey.isPressed)
            {
                myrigidBody2D.linearVelocity = new Vector2(myrigidBody2D.linearVelocity.x, jumpForce);
            }
        }
    }

    void CambiarEscenaLuegoDeMuerte()
    {
        SceneManager.LoadScene(1);
    }

    void ProcesarMovimiento()
    {
        myrigidBody2D.linearVelocity = new Vector2(movimiento.x * velocidad, myrigidBody2D.linearVelocity.y);
    }

    private IEnumerator Dash()
    {
        float localDashingPower;

        if (!facingRight)
        {
            localDashingPower = -dashingPower;
        }
        else
        {
            localDashingPower = dashingPower;
        }

        canDash = false;
        isDashing = true;

        float ogGravity = myrigidBody2D.gravityScale;
        myrigidBody2D.gravityScale = 0;
        myrigidBody2D.linearVelocity = new Vector2(localDashingPower, 0f);

        yield return new WaitForSeconds(dashingTime);

        myrigidBody2D.gravityScale = ogGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Suelo"))
        {
            onFloor = true;
        }

        if (col.CompareTag("Obstaculo"))
        {
            Debug.Log("bonk");
            vida -= 10; 
            if (vida <= 0)
            {
                youDied = true;
                SceneManager.LoadScene(4);
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Suelo"))
        {
            onFloor = false;
        }
    }
}