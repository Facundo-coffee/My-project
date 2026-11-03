using UnityEngine;

public class Enemigo2D : MonoBehaviour
{
    public Transform jugador;
    public float velocidad = 4f;
    public float distanciaFrenado = 1.5f;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Guardamos la referencia al componente que dibuja el sprite
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (jugador == null) return;

        float distancia = Vector2.Distance(transform.position, jugador.position);

        if (distancia > distanciaFrenado)
        {
            Vector2 posicionObjetivo = new Vector2(jugador.position.x, jugador.position.y);
            transform.position = Vector2.MoveTowards(transform.position, posicionObjetivo, velocidad * Time.deltaTime);

            GirarHaciaJugador();
        }
    }

    void GirarHaciaJugador()
    {
        // Si el jugador está a la izquierda, voltea el sprite. Si no, lo deja normal.
        if (jugador.position.x < transform.position.x)
        {
            spriteRenderer.flipX = true;  // Invierte la imagen horizontalmente
        }
        else
        {
            spriteRenderer.flipX = false; // Mantiene la imagen original
        }
    }
}