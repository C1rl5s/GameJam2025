using UnityEngine;
using UnityEngine.SceneManagement;

public class inicioJuego : MonoBehaviour
{
    public int numeroEscena = 2;

    public void CambiarEscena()
    {
        SceneManager.LoadScene(numeroEscena);
    }

    public void SalirDelJuego()
    {
        Application.Quit();
    }
    
}