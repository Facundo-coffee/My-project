using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // Importante para gestionar escenas

public class CambiarEscena : MonoBehaviour
{
    [SerializeField] private string escenaDestino;

    private bool transicionIniciada;
    private bool escenaCargada;

    // Método público para que el botón pueda detectarlo
    public void CargarEscena(string nombre)
    {
        SceneManager.LoadScene(nombre);
    }

    public void IniciarTransicion()
    {
        CargarEscenaAlFinalizarTransicion();
    }

    public void CargarEscenaAlFinalizarTransicion()
    {
        if (transicionIniciada || escenaCargada)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(escenaDestino))
        {
            Debug.LogError("Configura la escena destino.", this);
            return;
        }

        TransicionEscena transicion = FindFirstObjectByType<TransicionEscena>();

        if (transicion == null)
        {
            Debug.LogError("No se encontró un componente de transición en la escena.", this);
            return;
        }

        transicionIniciada = true;
        StartCoroutine(CargarEscenaDespuesDeTransicion(transicion));
    }

    private IEnumerator CargarEscenaDespuesDeTransicion(TransicionEscena transicion)
    {
        yield return transicion.EjecutarSalida();
        escenaCargada = true;
        CargarEscena(escenaDestino);
    }
}