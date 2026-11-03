using UnityEngine;

// Adjuntar este script a los objetos "Media luna" o "Mate" (o cualquier pickup curativo).
// Al entrar en contacto con el jugador (tag "Player" por defecto) aplicará curación y se destruirá.
public class CurativoPickup : MonoBehaviour
{
    [Header("Curación")]
    public float cantidadCuracion = 25f;

    [Header("Configuración")]
    public string tagJugador = "Player";
    public bool destruirAlRecoger = true;

    public AudioSource audioSource; // Referencia al AudioSource del pickup SFX
    public AudioClip clipCuracion; // Referencia al AudioClip de curación

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(tagJugador)) return;

        // Intentar obtener el componente SaludJugador en el collider o en sus padres
        SaludJugador salud = collision.GetComponent<SaludJugador>();
        if (salud == null) salud = collision.GetComponentInParent<SaludJugador>();

        if (salud != null)
        {
            salud.Curar(cantidadCuracion);
            ReproducirSonidoCuracion();
        }

        if (destruirAlRecoger)
        {
            Destroy(gameObject);
        }
    }

    public void ReproducirSonidoCuracion()
    {
        if (audioSource == null) return;

        if (clipCuracion != null)
        {
            audioSource.PlayOneShot(clipCuracion);
        }
        else
        {
            audioSource.Play();
        }
    }
}
