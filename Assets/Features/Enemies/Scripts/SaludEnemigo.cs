using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SaludEnemigo : MonoBehaviour
{
    [Header("Configuración de Salud")]
    public float vidaMaxima = 50f;
    public float vidaActual;
    public Slider barraDeVida;

    [Header("Efectos de Daño")]
    public float duracionTambaleo = 0.15f;
    public float fuerzaTambaleo = 0.1f;

    [Header("Efectos de Muerte")]
    public float tiempoDesvanecimiento = 1f;

    private SpriteRenderer spriteRenderer;
    private Vector3 posicionOriginal;
    private bool estaMuerto = false;

    public bool EstaMuerto => estaMuerto;
    public event Action<SaludEnemigo> AlMorir;

    void Start()
    {
        vidaActual = vidaMaxima;
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (barraDeVida != null)
        {
            barraDeVida.maxValue = vidaMaxima;
            barraDeVida.value = vidaActual;
        }
    }

    public void RecibirDano(float cantidad)
    {
        if (estaMuerto) return;

        vidaActual -= cantidad;
        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMaxima);

        if (barraDeVida != null)
        {
            barraDeVida.value = vidaActual;
        }

        // Ejecuta la animación por código de parpadeo y tambaleo
        StartCoroutine(EfectoTambaleoYColor());

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    private void Morir()
    {
        estaMuerto = true;

        // Desactivar colisiones y física para que no vuelva a dañar a Juancito ni reciba más golpes
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = false;

        // Asegurar que no conserve velocidad y detener otros componentes que puedan moverlo
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.simulated = false;
        }

        // Desactivar otros Behaviour (IA, scripts de movimiento, etc.) excepto este script
        var behaviours = GetComponents<Behaviour>();
        foreach (var b in behaviours)
        {
            if (b != this) b.enabled = false;
        }

        // Desactivar Animator si existe para que las animaciones no muevan el transform
        Animator animator = GetComponent<Animator>();
        if (animator != null) animator.enabled = false;

        // Ocultar la barra de vida
        if (barraDeVida != null)
        {
            barraDeVida.gameObject.SetActive(false);
        }

        AlMorir?.Invoke(this);

        // Iniciar la secuencia de muerte gradual
        StartCoroutine(EfectoMuerte());
    }

    // Animación de tambaleo + flash rojo al ser golpeado
    private IEnumerator EfectoTambaleoYColor()
    {
        posicionOriginal = transform.localPosition;
        Color colorOriginal = spriteRenderer.color;

        // Cambiar a color rojo al recibir el golpe
        spriteRenderer.color = Color.red;

        float tiempoPasado = 0f;
        while (tiempoPasado < duracionTambaleo)
        {
            // Mueve aleatoriamente la posición para simular el tambaleo
            float offsetOtros = UnityEngine.Random.Range(-1f, 1f) * fuerzaTambaleo;
            transform.localPosition = posicionOriginal + new Vector3(offsetOtros, 0, 0);

            tiempoPasado += Time.deltaTime;
            yield return null;
        }

        // Restaurar estado original
        transform.localPosition = posicionOriginal;
        spriteRenderer.color = colorOriginal;
    }

    // Animación de muerte: Desvanecimiento suave (Fade Out) por código
    private IEnumerator EfectoMuerte()
    {
        Color colorInicial = spriteRenderer.color;
        float tiempoPasado = 0f;

        while (tiempoPasado < tiempoDesvanecimiento)
        {
            tiempoPasado += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, tiempoPasado / tiempoDesvanecimiento);

            // Disminuye la opacidad gradualmente
            spriteRenderer.color = new Color(colorInicial.r, colorInicial.g, colorInicial.b, alpha);

            yield return null;
        }

        // Finalmente elimina el GameObject de la escena
        Destroy(gameObject);
    }
}
