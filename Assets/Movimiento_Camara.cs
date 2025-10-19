using UnityEngine;

public class Movimiento_Camara : MonoBehaviour
{
    public Transform objetivo;       
    public Vector3 offset = new Vector3(0, 0, -10); 
    public float suavizado = 5f;     

    void LateUpdate()
    {
        if (objetivo != null)
        {
            Vector3 posicionDeseada = objetivo.position + offset;
            Vector3 posicionSuavizada = Vector3.Lerp(transform.position, posicionDeseada, suavizado * Time.deltaTime);
            transform.position = posicionSuavizada;
        }
    }
}

