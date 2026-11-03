using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class CombateJugador : MonoBehaviour
{
    [Header("Configuración de Ataque")]
    public Transform controladorAtaque; 
    public float radioAtaque = 0.5f;     
    public float danoAtaque = 25f;       
    public LayerMask capaEnemigo;        

    [Header("Efecto visual de ataque")]
    [Tooltip("GameObject del efecto, asignado previamente y desactivado al iniciar")]
    public GameObject efectoAtaque;
    [Tooltip("Tiempo en segundos antes de desactivar el efecto")]
    public float duracionEfecto = 0.6f;
    private Coroutine desactivarEfectoCoroutine;

    [Header("Tiempo entre Ataques")]
    public float tiempoEntreAtaques = 0.5f;
    private float tiempoSiguienteAtaque = 0f;
    public event Action AtaqueIniciado;
    private InputSystem_Actions inputActions;
    private InputAction attackAction;

    void Awake()
    {
        inputActions = new InputSystem_Actions();
        attackAction = inputActions.Player.Attack;
    }

    void OnEnable()
    {
        inputActions?.Player.Enable();
    }

    void OnDisable()
    {
        inputActions?.Player.Disable();
    }

    void OnDestroy()
    {
        inputActions?.Dispose();
    }

    void Update()
    {
        if (Time.time >= tiempoSiguienteAtaque)
        {
            if (attackAction.WasPressedThisFrame())
            {
                Atacar();
                tiempoSiguienteAtaque = Time.time + tiempoEntreAtaques;
            }
        }
    }

    private void Atacar()
    {
        AtaqueIniciado?.Invoke();

        if (efectoAtaque != null && controladorAtaque != null)
        {
            efectoAtaque.transform.position = controladorAtaque.position;
            efectoAtaque.SetActive(true);

            if (desactivarEfectoCoroutine != null)
            {
                StopCoroutine(desactivarEfectoCoroutine);
            }

            desactivarEfectoCoroutine = StartCoroutine(DesactivarEfectoAtaque());
        }

        if (controladorAtaque == null) return;

        Collider2D[] objetosDetectados = Physics2D.OverlapCircleAll(controladorAtaque.position, radioAtaque, capaEnemigo);

        foreach (Collider2D enemigo in objetosDetectados)
        {
            SaludEnemigo saludEnemigo = enemigo.GetComponent<SaludEnemigo>();
            if (saludEnemigo != null)
            {
                saludEnemigo.RecibirDano(danoAtaque);
            }
        }
    }

    private IEnumerator DesactivarEfectoAtaque()
    {
        yield return new WaitForSeconds(duracionEfecto);
        efectoAtaque.SetActive(false);
        desactivarEfectoCoroutine = null;
    }

    private void OnDrawGizmosSelected()
    {
        if (controladorAtaque == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(controladorAtaque.position, radioAtaque);
    }
}