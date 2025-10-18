using UnityEngine;

public class logicaSaltoSuelo : MonoBehaviour
{
    public BoxCollider2D triggerBox;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (triggerBox == null)
        {
            triggerBox = gameObject.AddComponent<BoxCollider2D>();
            triggerBox.isTrigger = true;  // Convertirlo en Trigger

            // Ajusta la posición del Trigger Box encima del suelo
            triggerBox.offset = new Vector2(0, 0.5f);  // Puedes ajustar esto dependiendo de la altura de tu suelo
            triggerBox.size = new Vector2(1, 0.2f);  // Ajusta el tamaño según necesites
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
