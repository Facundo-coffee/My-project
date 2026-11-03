using UnityEngine;
using UnityEngine.UI;

public class SaludJugador : MonoBehaviour
{
    [Header("Configuración de Salud")]
    public float vidaMaxima = 100f;
    public float vidaActual;
    public Slider barraDeVida;

    [Header("Configuración de UI")]
    public GameObject textoGameOver; // Arrastra tu texto de UI aquí desde la jerarquía

    [Header("Configuración de Daño")]
    [Tooltip("Cantidad de vida que pierde por segundo mientras el enemigo lo toca")]
    public float danoPorSegundo = 25f;
    [Header("Cooldown y Sacudida al recibir daño")]
    [Tooltip("Tiempo mínimo entre daños consecutivos")]
    public float cooldownRecibirDano = 0.5f;
    [Tooltip("Duración de la sacudida cuando recibe daño")]
    public float duracionSacudida = 0.12f;
    [Tooltip("Fuerza máxima del desplazamiento de la sacudida")]
    public float fuerzaSacudida = 0.08f;

    private float tiempoUltimoDano = -100f;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (barraDeVida != null)
        {
            barraDeVida.maxValue = vidaMaxima;
            barraDeVida.value = vidaActual;
        }

        // Asegurarse de que el texto esté oculto al inicio
        if (textoGameOver != null)
        {
            textoGameOver.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemigo"))
        {
            RecibirDano(danoPorSegundo * Time.deltaTime);
        }
    }

    public void RecibirDano(float cantidad)
    {
        // Aplicar cooldown para evitar recibir daño demasiado frecuentemente
        if (Time.time - tiempoUltimoDano < cooldownRecibirDano) return;
        tiempoUltimoDano = Time.time;

        // Iniciar sacudida visual
        StartCoroutine(SacudirJugador(duracionSacudida, fuerzaSacudida));

        vidaActual -= cantidad;
        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMaxima);

        if (barraDeVida != null)
        {
            barraDeVida.value = vidaActual;
        }

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    // Método público para curar al jugador
    public void Curar(float cantidad)
    {
        vidaActual += cantidad;
        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMaxima);

        if (barraDeVida != null)
        {
            barraDeVida.value = vidaActual;
        }
    }

    // Sacudida simple del transform local para dar feedback al recibir daño
    private System.Collections.IEnumerator SacudirJugador(float duracion, float fuerza)
    {
        Vector3 posicionOriginal = transform.localPosition;
        float tiempoPasado = 0f;
        Color colorOriginal = Color.white;
        if (spriteRenderer != null) colorOriginal = spriteRenderer.color;

        // Cambiar a rojo mientras sacude
        if (spriteRenderer != null) spriteRenderer.color = Color.red;

        while (tiempoPasado < duracion)
        {
            float offsetX = Random.Range(-1f, 1f) * fuerza;
            float offsetY = Random.Range(-1f, 1f) * (fuerza * 0.6f);
            transform.localPosition = posicionOriginal + new Vector3(offsetX, offsetY, 0f);

            tiempoPasado += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = posicionOriginal;

        // Restaurar color original
        if (spriteRenderer != null) spriteRenderer.color = colorOriginal;
    }

    private void Morir()
    {
        Debug.Log("¡Juancito ha muerto!");

        // Muestra el texto "Te moriste"
        if (textoGameOver != null)
        {
            textoGameOver.SetActive(true);
        }

        // Desactiva el Sprite y los controles para que no siga moviéndose,
        // sin destruir el objeto completo para evitar errores de referencias
        GetComponent<SpriteRenderer>().enabled = false;
        
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        MonoBehaviour movimiento = GetComponent("Movimiento2D") as MonoBehaviour;
        if (movimiento != null) movimiento.enabled = false;
    }
}