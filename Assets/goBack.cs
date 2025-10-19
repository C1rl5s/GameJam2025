using UnityEngine;
using UnityEngine.SceneManagement;

public class BotonSimple : MonoBehaviour
{
    // Color normal y cuando pasas el mouse
    public Color colorNormal = Color.white;
    public Color colorHover = Color.yellow;
    public Color colorClick = Color.green;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = colorNormal;
    }

    void OnMouseEnter()
    {
        spriteRenderer.color = colorHover;
    }

    void OnMouseExit()
    {
        spriteRenderer.color = colorNormal;
    }

    // Cuando haces clic en el objeto
    void OnMouseDown()
    {
        spriteRenderer.color = colorClick;
        SceneManager.LoadScene(1);
    }

    void OnMouseUp()
    {
        spriteRenderer.color = colorHover;
    }
}