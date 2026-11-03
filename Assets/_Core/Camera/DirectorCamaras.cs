using System;
using Unity.Cinemachine;
using UnityEngine;

public class DirectorCamaras : MonoBehaviour
{
    [Header("Cámaras de Cinemachine")]
    [Tooltip("Cámara principal que sigue al personaje.")]
    [SerializeField] private CinemachineCamera camaraPrincipal;

    [Tooltip("Cámara estática de combate por defecto (opcional si la ZonaCombate provee la suya).")]
    [SerializeField] private CinemachineCamera camaraCombatePorDefecto;

    [Header("Configuración de Prioridades")]
    [SerializeField] private int prioridadActiva = 20;
    [SerializeField] private int prioridadInactiva = 0;

    [Tooltip("Si está activo, habilita/deshabilita el GameObject de la cámara de combate además de cambiar su prioridad.")]
    [SerializeField] private bool gestionarActivacionGameObject = false;

    private CinemachineCamera camaraCombateActual;

    public event Action<CinemachineCamera> CamaraActivaCambiada;
    public event Action<bool> ModoCombateCambiado;

    public CinemachineCamera CamaraPrincipal => camaraPrincipal;
    public CinemachineCamera CamaraCombateActual => camaraCombateActual;
    public bool EnModoCombate { get; private set; }

    private void Start()
    {
        InicializarCamaras();
    }

    private void InicializarCamaras()
    {
        if (camaraCombatePorDefecto != null)
        {
            EstablecerPrioridad(camaraCombatePorDefecto, prioridadInactiva);

            if (gestionarActivacionGameObject)
            {
                camaraCombatePorDefecto.gameObject.SetActive(false);
            }
        }

        if (camaraPrincipal != null)
        {
            if (gestionarActivacionGameObject && !camaraPrincipal.gameObject.activeSelf)
            {
                camaraPrincipal.gameObject.SetActive(true);
            }

            EstablecerPrioridad(camaraPrincipal, prioridadActiva);
            camaraPrincipal.Prioritize();
        }

        EnModoCombate = false;
    }

    /// <summary>
    /// Transiciona desde la cámara principal hacia una cámara estática de combate.
    /// </summary>
    public void ActivarCamaraCombate(CinemachineCamera camaraCombate = null)
    {
        CinemachineCamera objetivo = camaraCombate != null ? camaraCombate : camaraCombatePorDefecto;

        if (objetivo == null)
        {
            Debug.LogWarning("DirectorCamaras: No se asignó una cámara de combate para activar.", this);
            return;
        }

        if (camaraCombateActual != null && camaraCombateActual != objetivo)
        {
            EstablecerPrioridad(camaraCombateActual, prioridadInactiva);

            if (gestionarActivacionGameObject)
            {
                camaraCombateActual.gameObject.SetActive(false);
            }
        }

        camaraCombateActual = objetivo;

        if (camaraPrincipal != null)
        {
            EstablecerPrioridad(camaraPrincipal, prioridadInactiva);
        }

        if (!camaraCombateActual.gameObject.activeSelf)
        {
            camaraCombateActual.gameObject.SetActive(true);
        }

        EstablecerPrioridad(camaraCombateActual, prioridadActiva);
        camaraCombateActual.Prioritize();

        EnModoCombate = true;
        ModoCombateCambiado?.Invoke(true);
        CamaraActivaCambiada?.Invoke(camaraCombateActual);
    }

    /// <summary>
    /// Transiciona nuevamente a la cámara principal que sigue al jugador.
    /// </summary>
    public void VolverACamaraPrincipal()
    {
        if (camaraCombateActual != null)
        {
            EstablecerPrioridad(camaraCombateActual, prioridadInactiva);

            if (gestionarActivacionGameObject)
            {
                camaraCombateActual.gameObject.SetActive(false);
            }

            camaraCombateActual = null;
        }

        if (camaraCombatePorDefecto != null)
        {
            EstablecerPrioridad(camaraCombatePorDefecto, prioridadInactiva);

            if (gestionarActivacionGameObject)
            {
                camaraCombatePorDefecto.gameObject.SetActive(false);
            }
        }

        if (camaraPrincipal != null)
        {
            if (!camaraPrincipal.gameObject.activeSelf)
            {
                camaraPrincipal.gameObject.SetActive(true);
            }

            EstablecerPrioridad(camaraPrincipal, prioridadActiva);
            camaraPrincipal.Prioritize();
            CamaraActivaCambiada?.Invoke(camaraPrincipal);
        }

        EnModoCombate = false;
        ModoCombateCambiado?.Invoke(false);
    }

    /// <summary>
    /// Permite suscribir directamente el director a un evento booleano de estado de combate.
    /// </summary>
    public void CambiarEstadoCombate(bool enCombate)
    {
        if (enCombate)
        {
            ActivarCamaraCombate();
        }
        else
        {
            VolverACamaraPrincipal();
        }
    }

    private static void EstablecerPrioridad(CinemachineCamera camara, int prioridad)
    {
        if (camara == null) return;
        camara.Priority = prioridad;
    }
}
