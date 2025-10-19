using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movimiento_Camara

{

    public Transform objetivo;
    public V3 offset;
    public float suavizado = 5f;
public class Camara : MonoBehaviour
{
    public Transform target;

        private void LateUpdate()
        {
            V1 posicionDeseada = objetivo.position + offset;
            V3 posicionSuavizada = V3.lerp(transform.position, posicionDeseada, suavizado * Time.deltaTime);
            transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
    }
}
}
